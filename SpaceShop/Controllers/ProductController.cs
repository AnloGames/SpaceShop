using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpaceShop_DataMigrations;
using SpaceShop_Models;
using System.IO;
using SpaceShop_Utility;
using NuGet.Packaging.Signing;
using LogicService.IAdapter;
using LogicService.Dto;
using LogicService.Dto.ViewModels;
using LogicService.Service.IService;

namespace SpaceShop.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class ProductController : Controller
    {
        readonly IProductService productService;
        readonly ICategoryService categoryService;
        readonly IMyModelService myModelService;
        public ProductController(ICategoryService categoryService, IMyModelService myModelService, IProductService productService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.myModelService = myModelService;
        }
        public IActionResult Index(int? CategoryId)
        {

            IEnumerable<ProductDto> products = productService.GetProducts(CategoryId);

            return View(products);
        }
        public IActionResult CreateEdit(int? id)
        {
            ProductDto product = new ProductDto();
            if (id != null)
            {
                //edit
                product = productService.GetProduct((int)id);
                if (product == null)
                {
                    return NotFound();
                }
            }
            ProductCreation data = new ProductCreation(product, categoryService.GetAllCategories(), myModelService.GetMyModels());
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEdit(ProductDto product, int[] MyModelsId)
        {

            var files = HttpContext.Request.Form.Files;
            product = productService.UploadImage(files, product);
            if (product.Id == 0)
            {
                product = productService.SetShopCount(product, count: 1);
            }
            else
            {
                product = productService.SetShopCount(product);
                productService.RemoveProductMyModelConnections(product.Id);
            }
            product.ShortDescription = "NONE";
            product = productService.CompleteProductCreation(product);
            productService.CreateProductMyModelConnections(product, MyModelsId);
            return RedirectToAction("Index");
        }

        public IActionResult ChangeCount(int id)
        {
            ProductDto product = productService.GetProduct(id);
            productService.ChangeProductShortDescription(product);
            return View(id);
        }

        [HttpPost]
        public IActionResult ChangeCount(int id, int count)
        {
            productService.ChangeProductShopCount(id, count);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            ProductCreation productCreation = productService.CreateProductDeleteViewModel(id);

            return View(productCreation);
        }

        [HttpPost]
        public IActionResult Delete(ProductDto product)
        {

            if (product == null)
            {
                return NotFound();
            }
            productService.RemoveProductMyModelConnections(product.Id);
            productService.DeleteProduct(product);
            return RedirectToAction("Index");
        }
    }
}