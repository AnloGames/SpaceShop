using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using LogicService.IRepository;
using SpaceShop_Models;
using SpaceShop_Utility;
using LogicService.IAdapter;
using LogicService.Dto;
using ModelAdapter.Adapter;

namespace SpaceShop.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class CategoryController : Controller
    {
        //public ApplicationDbContext database;
        private ICategoryAdapter categoryAdapter;

        public CategoryController(ICategoryAdapter categoryAdapter)
        {
            this.categoryAdapter = categoryAdapter;
        }

        public IActionResult Index()
        {
            IEnumerable<CategoryDto> categories = categoryAdapter.GetAll();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryDto category)
        {
            if (ModelState.IsValid)
            {
                category.Id = 0;
                categoryAdapter.Add(category);
                categoryAdapter.Save();
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

            CategoryDto category = categoryAdapter.Find((int)id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryDto category)
        {
            if (ModelState.IsValid)
            {
                categoryAdapter.Update(category);
                categoryAdapter.Save();
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

            CategoryDto category = categoryAdapter.FirstOrDefaultById((int)id, isTracking: false);

            if (category == null)
            {
                return NotFound();
            }

            categoryAdapter.Remove(category);
            categoryAdapter.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Show(int id)
        {
            return RedirectToAction("Index", "Product", new { CategoryId = id});
        }
    }
}
