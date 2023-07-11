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
        IProductService productService;
        ICategoryService categoryService;
        IMyModelService myModelService;
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
            string wwwRoot = environment.WebRootPath;


            //копируем файл на сервер


            if (product.Id == 0)
            {
                string upload = wwwRoot + PathManager.ImageProductPath;
                string imageName = Guid.NewGuid().ToString();

                if (files.Count > 0)
                {
                    string extension = Path.GetExtension(files[0].FileName);
                    string path = upload + imageName + extension;
                    using (var FileStream = new FileStream(path, FileMode.Create))
                    {
                        files[0].CopyTo(FileStream);
                    }
                    product.Image = imageName + extension;
                }
                else
                {
                    product.Image = "NONE.png";
                }

                product.ShopCount = 1;
            }
            else
            {
                foreach (var connection in connectionProductMyModelAdapter.GetAllByProductId(product.Id, isTracking: false))
                {
                    connectionProductMyModelAdapter.Remove(connection);
                    productAdapter.Save();
                }
                ProductDto NowProduct = productAdapter.FirstOrDefaultById(product.Id, isTracking: false);
                product.ShopCount = NowProduct.ShopCount;
                if (files.Count > 0)
                {
                    string upload = wwwRoot + PathManager.ImageProductPath;
                    string imageName = Guid.NewGuid().ToString();

                    string extension = Path.GetExtension(files[0].FileName);
                    string path = upload + imageName + extension;
                    string oldFile = upload + NowProduct.Image;

                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }
                    using (var FileStream = new FileStream(path, FileMode.Create))
                    {
                        files[0].CopyTo(FileStream);
                    }

                    product.Image = imageName + extension;
                }
                else
                {
                    product.Image = NowProduct.Image;
                }
            }
            CategoryDto productCategory = categoryAdapter.Find(product.CategoryId);
            product.ShortDescription = "Category: " + productCategory.Name + "; Tags: ";
            List<ConnectionProductMyModelDto> connections = new List<ConnectionProductMyModelDto>();
            foreach (int MyModelId in MyModelsId)
            {
                ConnectionProductMyModelDto connection = new ConnectionProductMyModelDto();
                connection.Id = 0;
                connection.MyModelId = MyModelId;
                connections.Add(connection);
                product.ShortDescription += myModelAdapter.FirstOrDefaultById(MyModelId).Name + ", ";
            }
            if (product.Id == 0)
            {
                product = productAdapter.AddAndChange(product);
            }
            else
            {
                productAdapter.Update(product);
            }
            productAdapter.Save();
            foreach (var connection in connections)
            {
                connection.ProductId = product.Id;
                connectionProductMyModelAdapter.Add(connection);
                connectionProductMyModelAdapter.Save();
            }
            return RedirectToAction("Index");
        }

        public IActionResult ChangeCount(int id)
        {
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
            ProductDto product = productAdapter.FirstOrDefaultById(id);
            IEnumerable<ConnectionProductMyModelDto> connections = connectionProductMyModelAdapter.GetAllByProductId(id);
            List<int> ids = new List<int>();
            foreach (var connection in connections)
            {
                ids.Add(connection.MyModelId);
            }
            IEnumerable<MyModelDto> connectedModels = myModelAdapter.GetAllByIdList(ids);
            ProductCreation productCreation = new ProductCreation(product, connectedModels);

            return View(productCreation);
        }

        [HttpPost]
        public IActionResult Delete(ProductDto product)
        {

            if (product == null)
            {
                return NotFound();
            }
            ProductDto NowProduct = productAdapter.FirstOrDefaultById(product.Id, isTracking: false);

            string wwwRoot = environment.WebRootPath;
            string upload = wwwRoot + PathManager.ImageProductPath;
            string oldFile = upload + NowProduct.Image;

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }
            foreach (var connection in connectionProductMyModelAdapter.GetAllByProductId(product.Id, isTracking: false))
            {
                connectionProductMyModelAdapter.Remove(connection);
                productAdapter.Save();
            }

            productAdapter.Remove(product);
            productAdapter.Save();
            return RedirectToAction("Index");
        }
    }
}