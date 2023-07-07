using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpaceShop_DataMigrations;
using SpaceShop_Models;
using SpaceShop_ViewModels;
using System.IO;
using SpaceShop_Utility;
using LogicService.IRepository;
using NuGet.Packaging.Signing;
using LogicService.IAdapter;
using LogicService.Dto;

namespace SpaceShop.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class ProductController : Controller
    {
        private ApplicationDbContext database;
        /*private IWebHostEnvironment environment;

        public ProductController(ApplicationDbContext database, IWebHostEnvironment environment)
        {
            this.database = database;
            this.environment = environment;
        }*/
        private IProductAdapter productAdapter;
        private IRepositoryMyModel repositoryMyModel;
        private IRepositoryConnectionProductMyModel repositoryConnectionProductMyModel;
        private IWebHostEnvironment environment;

        private ICategoryAdapter categoryAdapter;
        public ProductController(IWebHostEnvironment environment, IProductAdapter productAdapter, 
            IRepositoryMyModel repositoryMyModel, IRepositoryConnectionProductMyModel repositoryConnectionProductMyModel, 
            ApplicationDbContext database, ICategoryAdapter categoryAdapter)
        {
            this.environment = environment;
            this.productAdapter = productAdapter;
            this.repositoryMyModel = repositoryMyModel;
            this.repositoryConnectionProductMyModel = repositoryConnectionProductMyModel;
            this.database = database;
            this.categoryAdapter = categoryAdapter;
        }
        public IActionResult Index(int? CategoryId)
        {

            IEnumerable<ProductDto> products;
            if (CategoryId == null)
            {
                products = productAdapter.GetAll();
            }
            else
            {
                products = productAdapter.GetAllByCategoryId((int)CategoryId, isTracking: false);

            }
            return View(products);
        }
        public IActionResult CreateEdit(int? id)
        {
            ProductDto product = new ProductDto();
            if (id != null)
            {
                //edit
                product = productAdapter.FirstOrDefaultById((int)id, isTracking: false);
                if (product == null)
                {
                    return NotFound();
                }
            }
            ProductCreation data = new ProductCreation(product, categoryAdapter.GetAll(), repositoryMyModel.GetAll());
            return View(data);
            /*IEnumerable<SelectListItem> CategoriesList = database.Category.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            ViewBag.Categories = CategoriesList;
            return View(product);*/
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
                foreach (ConnectionProductMyModel connection in repositoryConnectionProductMyModel.GetAll(filter: (u => u.ProductId == product.Id), isTracking: false))
                {
                    repositoryConnectionProductMyModel.Remove(connection);
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
            foreach (int MyModelId in MyModelsId)
            {
                productAdapter.Save();
                ConnectionProductMyModel connection = new ConnectionProductMyModel();
                connection.Id = 0;
                connection.MyModelId = MyModelId;
                connection.ProductId = product.Id;
                repositoryConnectionProductMyModel.Add(connection);
                product.ShortDescription += repositoryMyModel.Find(MyModelId).Name + ", ";
            }
            if (product.Id == 0)
            {
                productAdapter.Add(product);
            }
            else
            {
                productAdapter.Update(product);
            }
            productAdapter.Save();
            return RedirectToAction("Index");
        }

        public IActionResult ChangeCount(int id)
        {
            return View(id);
        }

        [HttpPost]
        public IActionResult ChangeCount(int id, int count)
        {
            ProductDto product = productAdapter.FirstOrDefaultById(id, isTracking: false);
            product.ShopCount = count;
            productAdapter.Update(product);
            productAdapter.Save();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            ProductDto product = productAdapter.FirstOrDefaultById(id);
            IEnumerable<ConnectionProductMyModel> connections = repositoryConnectionProductMyModel.GetAll(filter: (u => u.ProductId == id));
            List<int> ids = new List<int>();
            foreach (ConnectionProductMyModel connection in connections)
            {
                ids.Add(connection.MyModelId);
            }
            IEnumerable<MyModel> connectedModels = repositoryMyModel.GetAll(filter: u => ids.Contains(u.Id));
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
            foreach (ConnectionProductMyModel connection in repositoryConnectionProductMyModel.GetAll((u => u.ProductId == product.Id)))
            {
                repositoryConnectionProductMyModel.Remove(connection);
                productAdapter.Save();
            }

            productAdapter.Remove(product);
            productAdapter.Save();
            return RedirectToAction("Index");
        }
    }
}