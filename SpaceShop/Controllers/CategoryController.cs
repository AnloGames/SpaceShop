using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_Models;
using SpaceShop_Utility;
namespace SpaceShop.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class CategoryController : Controller
    {
        //public ApplicationDbContext database;
        private IRepositoryCategory repositoryCategory;

        public CategoryController(IRepositoryCategory repositoryCategory)
        {
            this.repositoryCategory = repositoryCategory;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = repositoryCategory.GetAll();
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
                category.Id = 0;
                repositoryCategory.Add(category);
                repositoryCategory.Save();
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

            Category category = repositoryCategory.Find((int)id);

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
                repositoryCategory.Update(category);
                repositoryCategory.Save();
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

            Category category = repositoryCategory.Find((int)id);

            if (category == null)
            {
                return NotFound();
            }

            repositoryCategory.Remove(category);
            repositoryCategory.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Show(int id)
        {
            return RedirectToAction("Index", "Product", new { CategoryId = id});
        }
    }
}
