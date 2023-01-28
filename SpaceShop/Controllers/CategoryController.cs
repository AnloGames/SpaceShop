using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using SpaceShop.Data;
using SpaceShop_Models;
using SpaceShop_Utility;
namespace SpaceShop.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class CategoryController : Controller
    {
        public ApplicationDbContext database;

        public CategoryController(ApplicationDbContext database)
        {
            this.database = database;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = database.Category;
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                database.Category.Add(category);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category category = database.Category.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                database.Category.Update(category);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category category = database.Category.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            database.Category.Remove(category);
            database.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Show(int id)
        {
            return RedirectToAction("Index", "Product", new { CategoryId = id});
        }
    }
}
