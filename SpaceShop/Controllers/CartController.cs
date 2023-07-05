using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceShop_DataMigrations;
using SpaceShop_Models;
using SpaceShop_ViewModels;
using SpaceShop_Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore.Storage;
using LogicService.IRepository;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SpaceShop_Utility.BrainTree;
using Braintree;
using LogicService.Service.IService;

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


        IRepositoryOrderHeader repositoryOrderHeader;
        IRepositoryOrderDetail repositoryOrderDetail;

        IOrderService orderService;

        public CartController(IWebHostEnvironment environment, IEmailSender emailSender, 
            IRepositoryProduct repositoryProduct, IRepositoryApplicationUser repositoryApplicationUser,
            IRepositoryOrderHeader repositoryOrderHeader, IRepositoryOrderDetail repositoryOrderDetail,
            IBrainTreeBridge brainTreeBridge, IOrderService orderService)
        {
            this.environment = environment;
            this.emailSender = emailSender;
            this.repositoryProduct = repositoryProduct;
            this.repositoryApplicationUser = repositoryApplicationUser;
            this.repositoryOrderHeader = repositoryOrderHeader;
            this.repositoryOrderDetail = repositoryOrderDetail;
            this.brainTreeBridge = brainTreeBridge;
            this.orderService = orderService;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
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

            var claimsIdentity = (ClaimsIdentity)User.Identity;

            // если пользователь вошел в систему, то объект будет определен
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            applicationUser = repositoryApplicationUser.FirstOrDefault(x => x.Id == claim.Value);

            //Оплата
            var getWay = brainTreeBridge.GetGateWay();
            var tokenClient = getWay.ClientToken.Generate();
            ViewBag.TokenClient = tokenClient;

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
            ApplicationUser user = productUserViewModel.ApplicationUser;
            List<Product> productList = productUserViewModel.ProductList;
            orderService.SaveOrder(user, productList, collection);
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
        public IActionResult Clear()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }

}
