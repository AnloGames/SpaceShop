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

        public ProductController(ApplicationDbContext database)
        {
            this.database = database;
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
        public IActionResult CreateEdit(Product product)
        {
            product.Category = database.Category.Find(product.CategoryId);
            product.Image = "";
            if (!database.Product.Contains(product))
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
