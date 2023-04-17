﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaceShop_DataMigrations;
using SpaceShop_Models;
using SpaceShop_ViewModels;
using SpaceShop_Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore.Storage;
using SpaceShop_DataMigrations.Repository.IRepository;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SpaceShop_Utility.BrainTree;
using Braintree;

namespace SpaceShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private IWebHostEnvironment environment;
        IEmailSender emailSender;

        ProductUserViewModel productUserViewModel;

        IBrainTreeBridge brainTreeBridge;

        IRepositoryApplicationUser repositoryApplicationUser;
        IRepositoryProduct repositoryProduct;

        IRepositoryQueryHeader repositoryQueryHeader;
        IRepositoryQueryDetail repositoryQueryDetail;

        IRepositoryOrderHeader repositoryOrderHeader;
        IRepositoryOrderDetail repositoryOrderDetail;

        public CartController(IWebHostEnvironment environment, IEmailSender emailSender, 
            IRepositoryProduct repositoryProduct, IRepositoryApplicationUser repositoryApplicationUser, 
            IRepositoryQueryHeader repositoryQueryHeader, IRepositoryQueryDetail repositoryQueryDetail,
            IRepositoryOrderHeader repositoryOrderHeader, IRepositoryOrderDetail repositoryOrderDetail,
            IBrainTreeBridge brainTreeBridge)
        {
            this.environment = environment;
            this.emailSender = emailSender;
            this.repositoryProduct = repositoryProduct;
            this.repositoryApplicationUser = repositoryApplicationUser;
            this.repositoryQueryHeader = repositoryQueryHeader;
            this.repositoryQueryDetail = repositoryQueryDetail;
            this.repositoryOrderHeader = repositoryOrderHeader;
            this.repositoryOrderDetail = repositoryOrderDetail;
            this.brainTreeBridge = brainTreeBridge;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            TempData[PathManager.Success] = User.IsInRole(PathManager.AdminRole).ToString();
            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<List<Cart>>(PathManager.SessionCart);

                // хотим получить каждый товар из корзины
            }

            // получаем лист id товаров
            List<int> productsIdInCart = cartList.Select(x => x.ProductId).ToList();

            // извлекаем сами продукты по списку id
            IEnumerable<Product> productList = repositoryProduct.GetAll(x => productsIdInCart.Contains(x.Id));
            foreach (Product product in productList)
            {
                product.TempCount = cartList.FirstOrDefault(x => x.ProductId == product.Id).TempCount;
            }

            return View(productList);
        }

        public IActionResult Remove(int id)
        {
            // удаление из корзины
            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<List<Cart>>(PathManager.SessionCart);
            }

            cartList.Remove(cartList.FirstOrDefault(x => x.ProductId == id));

            // переназначение сессии
            HttpContext.Session.Set(PathManager.SessionCart, cartList);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Summary()
        {
            ApplicationUser applicationUser;

            if (User.IsInRole(PathManager.AdminRole))
            {
                if (HttpContext.Session.Get<int>(PathManager.SessionQuery) != 0)
                {
                    QueryHeader queryHeader = repositoryQueryHeader.FirstOrDefault(
                        x => x.Id == HttpContext.Session.Get<int>(PathManager.SessionQuery));

                    applicationUser = new ApplicationUser()
                    {
                        Id = queryHeader.ApplicationUserId,
                        Email = queryHeader.Email,
                        PhoneNumber = queryHeader.PhoneNumber,
                        FullName = queryHeader.FullName
                    };
                }
                else
                {
                    applicationUser = new ApplicationUser();
                }
                //Оплата
                var getWay = brainTreeBridge.GetGateWay();
                var tokenClient = getWay.ClientToken.Generate();
                ViewBag.TokenClient = tokenClient;
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;

                // если пользователь вошел в систему, то объект будет определен
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                applicationUser = repositoryApplicationUser.FirstOrDefault(x => x.Id == claim.Value);
            }

            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<List<Cart>>(PathManager.SessionCart);
            }

            // извлекаем сами продукты по списку id
            List<Product> productList = new List<Product>();
            foreach (var cart in cartList)
            {
                Product product = repositoryProduct.Find(cart.ProductId);
                product.TempCount = cart.TempCount;
                productList.Add(product);
            }

            productUserViewModel = new ProductUserViewModel()
            {
                ApplicationUser = applicationUser,
                ProductList = productList
            };

            return View(productUserViewModel);
        }

        public async Task<IActionResult> InquiryConfirmation(IFormCollection collection, ProductUserViewModel productUserViewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ApplicationUser user = productUserViewModel.ApplicationUser;

            if (User.IsInRole(PathManager.AdminRole))
            {
                int totalPrice = 0;

                foreach (var item in productUserViewModel.ProductList)
                {
                    totalPrice += (int)(item.TempCount * item.Price);
                }
                //Work With Order
                OrderHeader orderHeader = new OrderHeader()
                {
                    AdminId = claim.Value,
                    DateOrder = DateTime.Now,
                    TotalPrice = totalPrice,
                    Status = PathManager.StatusPending,
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    City = user.City,
                    Street = user.Street,
                    House = user.House,
                    Apartment = user.Apartment,
                    PostalCode = user.PostalCode
                };
                repositoryOrderHeader.Add(orderHeader);
                repositoryOrderHeader.Save();


                foreach (var product in productUserViewModel.ProductList)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        OrderHeaderId = orderHeader.Id,
                        ProductId = product.Id,
                        Count = product.TempCount,
                        PricePerUnit = (int)product.Price    // !!! fix need 
                    };

                    repositoryOrderDetail.Add(orderDetail);
                }

                repositoryOrderDetail.Save();


                string nonce = collection["payment_method_nonce"];

                var request = new TransactionRequest
                {
                    Amount = 1, 
                    PaymentMethodNonce = nonce,
                    OrderId = "1",
                    Options = new TransactionOptionsRequest{ SubmitForSettlement = true }
                };

                var getWay = brainTreeBridge.GetGateWay();

                var resultTransaction = getWay.Transaction.Sale(request);

                var id = resultTransaction.Target.Id;
                var status = resultTransaction.Target.ProcessorResponseText;


                return View(true);
            }

            else
            {
                //Work With Query
                var path = environment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "templates" + Path.DirectorySeparatorChar.ToString() + "Inquiry.cshtml";
                string subject = "New sub";
                string bodyHtml = "";
                using (StreamReader reader = new StreamReader(path))
                {
                    bodyHtml = reader.ReadToEnd();
                }
                string textProducts = "";
                foreach (Product product in productUserViewModel.ProductList)
                {
                    Product nowProduct = repositoryProduct.Find(product.Id);
                    textProducts += $"Name: {nowProduct.Name}, Price: {nowProduct.Price}\n";
                }
                //Body
                string body = string.Format(bodyHtml, user.FullName, user.Email, user.PhoneNumber, textProducts);
                await emailSender.SendEmailAsync(user.Email, subject, body);
                QueryHeader queryHeader = new QueryHeader()
                {
                    ApplicationUserId = claim.Value,
                    QueryDate = DateTime.Now,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                };
                repositoryQueryHeader.Add(queryHeader);
                repositoryQueryHeader.Save();

                foreach (var item in productUserViewModel.ProductList)
                {
                    QueryDetail queryDetail = new QueryDetail()
                    {
                        ProductId = item.Id,
                        QueryHeaderId = queryHeader.Id
                    };
                    repositoryQueryDetail.Add(queryDetail);
                }
                repositoryQueryDetail.Save();
                HttpContext.Session.Clear();
            }

            return View(false);
        }

        public IActionResult Update(Product[] products)
        {
            List<Cart> cartList = new List<Cart>();
            foreach (var prod in products)
            {
                cartList.Add(new Cart()
                {
                    ProductId = prod.Id,
                    TempCount = prod.TempCount
                });
            }

            HttpContext.Session.Set(PathManager.SessionCart, cartList);

            return RedirectToAction("Index");
        }
    }
}
