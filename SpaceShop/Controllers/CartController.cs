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
        IPaymentService paymentService;
        IProductService productService;
        ICartService cartService;
        IApplicationUserService applicationUserService;

        public CartController(IWebHostEnvironment environment, IEmailSender emailSender, 
            IRepositoryProduct repositoryProduct, IRepositoryApplicationUser repositoryApplicationUser,
            IRepositoryOrderHeader repositoryOrderHeader, IRepositoryOrderDetail repositoryOrderDetail,
            IBrainTreeBridge brainTreeBridge, IOrderService orderService, IPaymentService paymentService,
            IProductService productService, ICartService cartService, IApplicationUserService applicationUserService)
        {
            this.environment = environment;
            this.emailSender = emailSender;
            this.repositoryProduct = repositoryProduct;
            this.repositoryApplicationUser = repositoryApplicationUser;
            this.repositoryOrderHeader = repositoryOrderHeader;
            this.repositoryOrderDetail = repositoryOrderDetail;
            this.brainTreeBridge = brainTreeBridge;
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.productService = productService;
            this.cartService = cartService;
            this.applicationUserService = applicationUserService;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Cart> cartList = cartService.GetSessionCartList(HttpContext).ToList();
            IEnumerable<Product> productList = productService.GetProductsInCart(cartList);

            return View(productList);
        }

        public IActionResult Remove(int id)
        {
            List<Cart> cartList = cartService.GetSessionCartList(HttpContext).ToList();

            cartList.Remove(cartList.FirstOrDefault(x => x.ProductId == id));

            HttpContext.Session.Set(PathManager.SessionCart, cartList);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Summary()
        {
            ApplicationUser applicationUser = applicationUserService.GetApplicationUserByIdentity(User);

            ViewBag.TokenClient = paymentService.GetTokenClient();

            List<Cart> cartList = cartService.GetSessionCartList(HttpContext).ToList();

            IEnumerable<Product> productList = productService.GetProductsInCart(cartList);

            productUserViewModel = new ProductUserViewModel()
            {
                ApplicationUser = applicationUser,
                ProductList = productList.ToList()
            };

            return View(productUserViewModel);
        }

        public async Task<IActionResult> InquiryConfirmation(IFormCollection collection, ProductUserViewModel productUserViewModel)
        {
            ApplicationUser user = productUserViewModel.ApplicationUser;
            List<Product> productList = productUserViewModel.ProductList;

            string transactionId = paymentService.GetTransactionId(collection);

            orderService.SaveOrder(user, productList, transactionId);
            HttpContext.Session.Clear();

            return View();
        }

        public IActionResult Update(Product[] products)
        {
            List<Cart> cartList = cartService.GetCartListByProducts(products).ToList();

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
