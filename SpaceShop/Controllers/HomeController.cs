using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceShop.Data;
using SpaceShop.Models;
using SpaceShop.ViewModels;
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
        public IActionResult Details(int id)
        {
            DetailsViewModel detailsViewModel = new DetailsViewModel(false, database.Product.Include(x => x.Category).Where(x => x.Id == id).FirstOrDefault());
            return View(detailsViewModel);
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