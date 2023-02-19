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

namespace SpaceShop.Controllers
{
    //[Authorize(Roles = PathManager.AdminRole)]
    public class ProductController : Controller
    {
        //private ApplicationDbContext database;
        private IRepositoryProduct repositoryProduct;
        private IRepositoryMyModel repositoryMyModel;
        private IRepositoryCategory repositoryCategory;
        private IRepositoryConnectionProductMyModel repositoryConnectionProductMyModel;
        private IWebHostEnvironment environment;

        public ProductController(IWebHostEnvironment environment, IRepositoryProduct repositoryProduct, IRepositoryMyModel repositoryMyModel, IRepositoryCategory repositoryCategory, IRepositoryConnectionProductMyModel repositoryConnectionProductMyModel)
        {
            this.environment = environment;
            this.repositoryProduct = repositoryProduct;
            this.repositoryMyModel = repositoryMyModel;
            this.repositoryCategory = repositoryCategory;
            this.repositoryConnectionProductMyModel = repositoryConnectionProductMyModel;
        }

        public IActionResult Index(int? CategoryId)
        {

            IEnumerable<Product> products = repositoryProduct.GetAll(); ;
            /*if (CategoryId == null)
            {
                products = repositoryProduct.GetAll();
            }
            else
            {
                products = database.Product.AsNoTracking().Where(p => p.CategoryId == CategoryId);

            }*/
            return View(products);
        }
        public IActionResult CreateEdit(int? id)
        {
            Product product = new Product();
            if (id != null)
            {
                //edit
                product = repositoryProduct.Find((int)id);
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

            product.ShortDescription = "";
            if (product.Id == 0)
            {
                string upload = wwwRoot + PathManager.ImageProductPath;
                string imageName = Guid.NewGuid().ToString();

                string extension = Path.GetExtension(files[0].FileName);

                string path = upload + imageName + extension;
                using (var FileStream = new FileStream(path, FileMode.Create))
                {
                    files[0].CopyTo(FileStream);
                }
                product.Image = imageName + extension;
                repositoryProduct.Add(product);
            }
            else
            {
                foreach (ConnectionProductMyModel connection in repositoryConnectionProductMyModel.GetAll(filter: u => u.ProductId == product.Id, isTracking:false))
                {
                    repositoryConnectionProductMyModel.Remove(connection);
                }
                Product NowProduct = repositoryProduct.FirstOrDefault(filter: u => u.Id == product.Id, isTracking: false);
                if (files.Count>0)
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
            repositoryProduct.Save();
            product.Category = repositoryCategory.Find(product.CategoryId);
            product.ShortDescription = "Category: " + product.Category.Name + "; MyModels: ";
            foreach (int MyModelId in MyModelsId)
            {
                repositoryConnectionProductMyModel.Save();
                ConnectionProductMyModel connection = new ConnectionProductMyModel();
                connection.Id = 0;
                connection.MyModelId = MyModelId;
                connection.ProductId = product.Id;
                repositoryConnectionProductMyModel.Add(connection);
                product.ShortDescription += repositoryMyModel.Find(MyModelId).Name + ", ";
            }
            repositoryProduct.Update(product);
            repositoryProduct.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Product product = repositoryProduct.FirstOrDefault(filter: u => u.Id == id, isTracking: false);
            /*product.Category = repositoryCategory.FirstOrDefault(u => u.Id == product.CategoryId, isTracking: false);
            IEnumerable<ConnectionProductMyModel> connections = repositoryConnectionProductMyModel.GetAll(filter: u => u.ProductId == id, isTracking: false);
            List<int> ids = new List<int>();
            foreach (ConnectionProductMyModel connection in connections)
            {
                ids.Add(connection.MyModelId);
            }
            IEnumerable<MyModel> connectedModels = repositoryMyModel.GetAll(filter: u => ids.Contains(u.Id), isTracking: false);
            ProductCreation productCreation = new ProductCreation(product, connectedModels);

            return View(productCreation);*/
            if (product == null)
            {
                return NotFound();
            }
            Product NowProduct = repositoryProduct.FirstOrDefault(filter: u => u.Id == product.Id, isTracking: false);

            string wwwRoot = environment.WebRootPath;
            string upload = wwwRoot + PathManager.ImageProductPath;
            string oldFile = upload + NowProduct.Image;

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }
            foreach (ConnectionProductMyModel connection in repositoryConnectionProductMyModel.GetAll(filter: u => u.ProductId == product.Id, isTracking: false))
            {
                repositoryConnectionProductMyModel.Remove(connection);
            }

            repositoryProduct.Remove(product);
            repositoryProduct.Save();
            repositoryConnectionProductMyModel.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {

            if (product == null)
            {
                return NotFound();
            }
            Product NowProduct = repositoryProduct.FirstOrDefault(filter: u => u.Id == product.Id, isTracking: false);

            string wwwRoot = environment.WebRootPath;
            string upload = wwwRoot + PathManager.ImageProductPath;
            string oldFile = upload + NowProduct.Image;

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }
            foreach (ConnectionProductMyModel connection in repositoryConnectionProductMyModel.GetAll(filter: u => u.ProductId == product.Id, isTracking: false))
            {
                repositoryConnectionProductMyModel.Remove(connection);
            }

            repositoryProduct.Remove(product);
            repositoryProduct.Save();
            repositoryConnectionProductMyModel.Save();
            return RedirectToAction("Index");
        }
    }
}
