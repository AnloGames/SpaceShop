using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using SpaceShop_Models;
using SpaceShop_Utility;
using LogicService.IAdapter;
using LogicService.Dto;
using LogicService.Service.IService;

namespace SpaceShop.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class CategoryController : Controller
    {
        //public ApplicationDbContext database;
        private ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult Index()
        {
            IEnumerable<CategoryDto> categories = categoryService.GetAllCategories();
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
            CategoryDto? createdCategory = categoryService.CreateCategory(ModelState.IsValid, category);
            if (createdCategory != null)
            {
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Edit(int id)
        {
            CategoryDto? category = categoryService.GetCategory(id);

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
            CategoryDto updatedCategory = categoryService.UpdateCategory(ModelState.IsValid, category);
            if (updatedCategory != null)
            {
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public IActionResult Delete(int id)
        {
            CategoryDto? category = categoryService.RemoveCategory(id);

            if (category == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Show(int id)
        {
            return RedirectToAction("Index", "Product", new { CategoryId = id});
        }
    }
}
