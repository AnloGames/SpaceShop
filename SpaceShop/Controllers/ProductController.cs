using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpaceShop.Data;
using SpaceShop.Models;
using SpaceShop.ViewModels;
using System.IO;

namespace SpaceShop.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext database;
        private IWebHostEnvironment environment;

        public ProductController(ApplicationDbContext database, IWebHostEnvironment environment)
        {
            this.database = database;
            this.environment = environment;
        }

        public IActionResult Index(int? CategoryId)
        {

            IEnumerable<Product> products;
            if (CategoryId == null)
            {
                products = database.Product;
            }
            else
            {
                products = database.Product.AsNoTracking().Where(p => p.CategoryId == CategoryId);

            }
            return View(products);
        }
        public IActionResult CreateEdit(int? id)
        {
            Product product = new Product();
            if (id != null)
            {
                //edit
                product = database.Product.Find(id);
                if (product == null)
                {
                    return NotFound();
                }
            }
            ProductCreation data = new ProductCreation(product, database.Category, database.MyModel);
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
        public IActionResult CreateEdit(Product product)
        {
            /*product.Category = database.Category.Find(product.CategoryId);
            product.Image = "";
            if (!database.Product.Contains(product))
            {
                database.Product.Add(product);
            }
            else
            {
                database.Product.Update(product);
            }
            database.SaveChanges();*/

            var files = HttpContext.Request.Form.Files;
            string wwwRoot = environment.WebRootPath;


            //копируем файл на сервер


            if (product.Id == 0)
            {
                database.Product.Add(product);
                //Потом сделать проверку на существование файла
                string upload = wwwRoot + PathManager.ImageProductPath;
                string imageName = Guid.NewGuid().ToString();

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
                Product NowProduct = database.Product.AsNoTracking().FirstOrDefault(u => u.Id == product.Id);
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
                database.Product.Update(product);
            }
            product.MyModel = database.MyModel.FirstOrDefault(u => u.Id == product.MyModelId);
            product.Category = database.Category.FirstOrDefault(u => u.Id == product.CategoryId);
            product.ShortDescription = "Category: " + product.Category.Name + ", MyModels: " + product.MyModel.Name;
            database.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Product product = database.Product.FirstOrDefault(u => u.Id == id);
            product.Category = database.Category.FirstOrDefault(u => u.Id == product.CategoryId);
            product.MyModel = database.MyModel.FirstOrDefault(U => U.Id == product.MyModelId);

            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {

            if (product == null)
            {
                return NotFound();
            }
            Product NowProduct = database.Product.AsNoTracking().FirstOrDefault(u => u.Id == product.Id);

            string wwwRoot = environment.WebRootPath;
            string upload = wwwRoot + PathManager.ImageProductPath;
            string oldFile = upload + NowProduct.Image;

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            database.Product.Remove(product);
            database.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
