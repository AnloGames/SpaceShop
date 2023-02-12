using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceShop_DataMigrations;
using SpaceShop_Models;
using SpaceShop_Utility;
using SpaceShop_ViewModels;
using System.Diagnostics;

namespace SpaceShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext database;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext database)
        {
            _logger = logger;
            this.database = database;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                products = database.Product,
                categories = database.Category

            };
            return View(homeViewModel);
        }
        public IActionResult Details(int? id)
        {
            List<Cart> cartList = new List<Cart>();
            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).ToList();
            }
            DetailsViewModel detailsViewModel = new DetailsViewModel(false, database.Product.Include(x => x.Category).Where(x => x.Id == id).FirstOrDefault());
            foreach (var item in cartList)
            {
                if (item.ProductId == id)
                {

                }detailsViewModel.IsInCart = true;
            }
            return View(detailsViewModel);
        }
        [HttpPost]
        public IActionResult Details(int id)
        {
            List<Cart> cartList = new List<Cart>();
            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count()>0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).ToList();
            }
            if (cartList.FirstOrDefault(x => x.ProductId == id) != null)
            {
                return RemoveFromCart(id);
            }
            cartList.Add(new Cart() { ProductId = id });
            HttpContext.Session.Set(PathManager.SessionCart, cartList);
            return RedirectToAction("Index");
        }
        public IActionResult RemoveFromCart(int id)
        {
            List<Cart> cartList = new List<Cart>();
            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).ToList();
            }
            var cart = cartList.FirstOrDefault(x=> x.ProductId == id);
            cartList.Remove(cart);
            HttpContext.Session.Set(PathManager.SessionCart, cartList);
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}