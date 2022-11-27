using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaceShop.Data;
using SpaceShop.Models;
using SpaceShop.ViewModels;

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
                products = database.Product.Where(p => p.CategoryId == CategoryId);
            }
            /*foreach (Product item in products)
            {
                item.Category = database.Category.FirstOrDefault(x => x.Id == item.CategoryId);
            }*/
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
            ProductCreation data = new ProductCreation(product, database.Category);
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

            //Потом сделать проверку на существование файла
            string upload = wwwRoot + PathManager.ImageProductPath;
            string imageName = Guid.NewGuid().ToString();

            string extension = Path.GetExtension(files[0].FileName);

            string path = upload + imageName + extension;
            //копируем файл на сервер
            using (var FileStream = new FileStream(path, FileMode.Create))
            {
                files[0].CopyTo(FileStream);
            }
            product.Image = path;

            if (product.Id == 0)
            {
                database.Product.Add(product);
            }
            else
            {
                database.Product.Update(product);
            }
            database.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product product = database.Product.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            database.Product.Remove(product);
            database.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
