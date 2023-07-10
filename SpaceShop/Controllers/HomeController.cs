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
    private ICategoryService categoryService;
    private IProductService productService;
    private IHomeService homeService;
    public HomeController(ILogger<HomeController> logger, IProductService productService, ICategoryService categoryService, IHomeService homeService)
    {
        _logger = logger;
        this.productService = productService;
        this.categoryService = categoryService;
        this.homeService = homeService;
    }

    public IActionResult Index()
    {
        HomeViewModel homeViewModel = homeService.CreateHomeViewModel();

        return View(homeViewModel);
    }

    public IActionResult Details(int id)
    {
        List<Cart> cartList = new List<Cart>();

        if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
            && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
        {
            cartList = HttpContext.Session.Get<List<Cart>>(PathManager.SessionCart);
        }


        DetailsViewModel detailsViewModel = new DetailsViewModel(false, productAdapter.FirstOrDefaultById(id, includeProperties: "Category"));
        // проверка на наличие товара в корзине
        // если товар есть, то меняем свойство
        foreach (var item in cartList)
        {
            if (item.ProductId == id)
            {
                detailsViewModel.IsInCart = true;
            }
        }

        return View(detailsViewModel);
    }

    [HttpPost]
    public IActionResult Details(int id, int count)
    {
        ProductDto product = productAdapter.Find(id);
        if (count <= product.ShopCount)
        {
            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<List<Cart>>(PathManager.SessionCart);
            }

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
        List<Cart> cartList = new List<Cart>();

        if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
            && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
        {
            cartList = HttpContext.Session.Get<List<Cart>>(PathManager.SessionCart);
        }

        // get product from cart
        var item = cartList.Single(x => x.ProductId == id);

        if (item != null)
        {
            cartList.Remove(item);
        }

        // SET SESSION
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
