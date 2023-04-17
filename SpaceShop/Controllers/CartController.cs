using System;
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

namespace SpaceShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private IWebHostEnvironment environment;
        IEmailSender emailSender;

        ProductUserViewModel productUserViewModel;

        IRepositoryApplicationUser repositoryApplicationUser;
        IRepositoryProduct repositoryProduct;

        IRepositoryQueryHeader repositoryQueryHeader;
        IRepositoryQueryDetail repositoryQueryDetail;

        public CartController(IWebHostEnvironment environment, IEmailSender emailSender, IRepositoryProduct repositoryProduct, IRepositoryApplicationUser repositoryApplicationUser, IRepositoryQueryHeader repositoryQueryHeader, IRepositoryQueryDetail repositoryQueryDetail)
        {
            this.environment = environment;
            this.emailSender = emailSender;
            this.repositoryProduct = repositoryProduct;
            this.repositoryApplicationUser = repositoryApplicationUser;
            this.repositoryQueryHeader = repositoryQueryHeader;
            this.repositoryQueryDetail = repositoryQueryDetail;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            TempData[PathManager.Success] = "Ok";
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

            if (true) //(User.IsInRole(PathManager.AdminRole))
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

        public async Task<IActionResult> InquiryConfirmation(string PhoneNumber, int[] ProductIds, string UserId)
        {
            List<Product> products = repositoryProduct.GetAll(x => ProductIds.Contains(x.Id)).ToList();
            ApplicationUser user = repositoryApplicationUser.FirstOrDefault(u => u.Id == UserId);
            var path = environment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "templates" + Path.DirectorySeparatorChar.ToString() + "Inquiry.cshtml";
            string subject = "New sub";
            string bodyHtml = "";
            using (StreamReader reader = new StreamReader(path))
            {
                bodyHtml = reader.ReadToEnd();
            }
            string textProducts = "";
            foreach (Product product in products)
            {
                textProducts += $"Name: {product.Name}, Price: {product.Price}\n";
            }
            //Body
            string body = string.Format(bodyHtml, user.FullName, user.Email, PhoneNumber, textProducts);
            await emailSender.SendEmailAsync(user.Email, subject, body);
            QueryHeader queryHeader = new QueryHeader()
            {
                ApplicationUserId = user.Id,
                QueryDate = DateTime.Now,
                FullName = user.FullName,
                PhoneNumber = PhoneNumber,
                Email = user.Email,
            };
            repositoryQueryHeader.Add(queryHeader);
            repositoryQueryHeader.Save();

            foreach (var item in products)
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
            return View();
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
