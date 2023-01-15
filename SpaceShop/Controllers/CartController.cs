using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceShop.Data;
using SpaceShop.Models;
using SpaceShop.ViewModels;
using SpaceShop.Utility;
using SpaceShop.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace SpaceShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        public ApplicationDbContext database;

        public ProductUserViewModel productUserViewModel;

        public CartController(ApplicationDbContext database)
        {
            this.database = database;
        }

        public IActionResult Index()
        {
            List<Cart> cartList = new List<Cart>();
            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).ToList();
            }

            List<int> productIdList = cartList.Select(x => x.ProductId).ToList();

            IEnumerable<Product> productList = database.Product.Where(x => productIdList.Contains(x.Id));

            return View(productList);
        }

        public IActionResult Remove(int id)
        {
            List<Cart> cartList = new List<Cart>();
            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).ToList();
            }
            var cart = cartList.FirstOrDefault(x => x.ProductId == id);
            cartList.Remove(cart);
            HttpContext.Session.Set(PathManager.SessionCart, cartList);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<Cart> cartList = new List<Cart>();
            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).ToList();
            }
            List<int> productIdList = cartList.Select(x => x.ProductId).ToList();

            IEnumerable<Product> productList = database.Product.Where(x => productIdList.Contains(x.Id));

            /*productUserViewModel = new ProductUserViewModel()
            {
                products = productList,
                applicationUser = database.ApplicationUser.FirstOrDefault
            };*/

            return View(productUserViewModel);
        }
    }
}
