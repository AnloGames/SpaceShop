using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaceShop.Data;
using SpaceShop.Models;
using SpaceShop.ViewModels;
using SpaceShop.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore.Storage;

namespace SpaceShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        ApplicationDbContext db;
        private IWebHostEnvironment environment;
        IEmailSender emailSender;

        ProductUserViewModel productUserViewModel;

        public CartController(ApplicationDbContext db, IWebHostEnvironment environment, IEmailSender emailSender)
        {
            this.db = db;
            this.environment = environment;
            this.emailSender = emailSender;
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
            IEnumerable<Product> productList = db.Product.Where(x => productsIdInCart.Contains(x.Id));

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
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            // если пользователь вошел в систему, то объект будет определен
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<List<Cart>>(PathManager.SessionCart);
            }

            // получаем лист id товаров
            List<int> productsIdInCart = cartList.Select(x => x.ProductId).ToList();

            // извлекаем сами продукты по списку id
            IEnumerable<Product> productList = db.Product.Where(x => productsIdInCart.Contains(x.Id));


            productUserViewModel = new ProductUserViewModel()
            {
                ApplicationUser = db.ApplicationUser.FirstOrDefault(x => x.Id == claim.Value),
                ProductList = productList
            };

            return View(productUserViewModel);
        }

        public async Task<IActionResult> InquiryConfirmation(int[] ProductIds, string UserId)
        {
            List<Product> products = db.Product.Where(x => ProductIds.Contains(x.Id)).ToList();
            ApplicationUser user = db.ApplicationUser.Find(UserId);
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
            string body = string.Format(bodyHtml, user.FullName, user.Email, user.PhoneNumber, textProducts);
            await emailSender.SendEmailAsync(user.Email, subject, body);
            HttpContext.Session.Clear();
            return View();
        }
    }
}
