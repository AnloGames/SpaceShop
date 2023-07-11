using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceShop_Models;
using SpaceShop_Utility;
using LogicService.IAdapter;
using LogicService.Dto;
using LogicService.Service;
using LogicService.Service.IService;
using LogicService.Dto.ViewModels;

namespace SpaceShop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IProductService productService;
    private IHomeService homeService;
    ICartService cartService;
    public HomeController(ILogger<HomeController> logger, IProductService productService, 
        IHomeService homeService, ICartService cartService)
    {
        _logger = logger;
        this.cartService = cartService;
        this.productService = productService;
        this.homeService = homeService;
    }

    public IActionResult Index()
    {
        HomeViewModel homeViewModel = homeService.CreateHomeViewModel();

        return View(homeViewModel);
    }

    public IActionResult Details(int id)
    {
        IEnumerable<Cart> cartList = cartService.GetSessionCartList(HttpContext).ToList();

        DetailsViewModel detailsViewModel = homeService.CreateDetailsViewModel(id, cartList);

        return View(detailsViewModel);
    }

    [HttpPost]
    public IActionResult Details(int id, int count)
    {
        int productShopCount = productService.GetProductShopCount(id);
        if (count <= productShopCount)
        {
            List<Cart> cartList = cartService.GetSessionCartList(HttpContext).ToList();

            cartList.Add(new Cart() { ProductId = id, TempCount = count });

            HttpContext.Session.Set(PathManager.SessionCart, cartList);

            return RedirectToAction("Index");
        }
        TempData[PathManager.Error] = "Not enough items in stock";

        return RedirectToAction("Details", id);
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int id)
    {
        List<Cart> cartList = cartService.GetSessionCartList(HttpContext).ToList();
        var item = cartList.Single(x => x.ProductId == id);
        if (item != null)
        {
            cartList.Remove(item);
        }

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
