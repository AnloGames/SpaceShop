using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpaceShop_DataMigrations;
using SpaceShop_Models;
using SpaceShop_ViewModels;
using System.IO;
using SpaceShop_Utility;
using SpaceShop_DataMigrations.Repository.IRepository;
using NuGet.Packaging.Signing;

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
        private IRepositoryProduct repositoryProduct;
        private IRepositoryMyModel repositoryMyModel;
        private IRepositoryCategory repositoryCategory;
        private IRepositoryConnectionProductMyModel repositoryConnectionProductMyModel;
        private IWebHostEnvironment environment;

        public ProductController(IWebHostEnvironment environment, IRepositoryProduct repositoryProduct, IRepositoryMyModel repositoryMyModel, IRepositoryCategory repositoryCategory, IRepositoryConnectionProductMyModel repositoryConnectionProductMyModel, ApplicationDbContext database)
        {
            this.environment = environment;
            this.repositoryProduct = repositoryProduct;
            this.repositoryMyModel = repositoryMyModel;
            this.repositoryCategory = repositoryCategory;
            this.repositoryConnectionProductMyModel = repositoryConnectionProductMyModel;
            this.database = database;
        }
        public IActionResult Index(int? CategoryId)
        {

            IEnumerable<Product> products;
            if (CategoryId == null)
            {
                products = repositoryProduct.GetAll();
            }
            else
            {
                products = repositoryProduct.GetAll(filter: (p => p.CategoryId == CategoryId), isTracking: false);

            }
            return View(products);
        }
        public IActionResult CreateEdit(int? id)
        {
            Product product = new Product();
            if (id != null)
            {
                //edit
                product = repositoryProduct.FirstOrDefault(isTracking: false, filter: (p => p.Id == id));
                if (product == null)
                {
                    return NotFound();
                }
            }
            ProductCreation data = new ProductCreation(product, repositoryCategory.GetAll(), repositoryMyModel.GetAll());
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
        public IActionResult CreateEdit(Product product, int[] MyModelsId)
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
                repositoryProduct.Add(product);
            }
            else
            {
                foreach (ConnectionProductMyModel connection in repositoryConnectionProductMyModel.GetAll(filter: (u => u.ProductId == product.Id), isTracking: false))
                {
                    repositoryConnectionProductMyModel.Remove(connection);
                    repositoryProduct.Save();
                }
                Product NowProduct = repositoryProduct.FirstOrDefault(filter: u => u.Id == product.Id, isTracking: false);
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
                repositoryProduct.Update(product);
            }
            product.Category = repositoryCategory.FirstOrDefault(filter: u => u.Id == product.CategoryId);
            product.ShortDescription = "Category: " + product.Category.Name + "; Tags: ";
            foreach (int MyModelId in MyModelsId)
            {
                repositoryProduct.Save();
                ConnectionProductMyModel connection = new ConnectionProductMyModel();
                connection.Id = 0;
                connection.MyModelId = MyModelId;
                connection.ProductId = product.Id;
                repositoryConnectionProductMyModel.Add(connection);
                product.ShortDescription += repositoryMyModel.Find(MyModelId).Name + ", ";
            }
            repositoryProduct.Save();
            return RedirectToAction("Index");
        }

        public IActionResult ChangeCount(int id)
        {
            return View(id);
        }

        [HttpPost]
        public IActionResult ChangeCount(int id, int count)
        {
            Product product = repositoryProduct.Find(id);
            product.ShopCount = count;
            repositoryProduct.Update(product);
            repositoryProduct.Save();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Product product = repositoryProduct.FirstOrDefault(filter: u => u.Id == id);
            product.Category = repositoryCategory.FirstOrDefault(filter: u => u.Id == product.CategoryId);
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
        public IActionResult Delete(Product product)
        {

            if (product == null)
            {
                return NotFound();
            }
            Product NowProduct = repositoryProduct.FirstOrDefault(filter: (u => u.Id == product.Id), isTracking: false);

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
                repositoryProduct.Save();
            }

            repositoryProduct.Remove(product);
            repositoryProduct.Save();
            return RedirectToAction("Index");
        }
    }
}