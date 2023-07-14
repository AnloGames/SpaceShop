using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceShop_Utility;
using LogicService.Models;
using LogicService.Service.IService;
using AutoMapper;
using SpaceShop.Models;

namespace SpaceShop.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class CategoryController : Controller
    {
        readonly ICategoryService categoryService;
        readonly IMapper mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            List<ControllerCategoryModel> categories = new List<ControllerCategoryModel>();
            foreach (var item in categoryService.GetAllCategories())
            {
                categories.Add(mapper.Map<ControllerCategoryModel>(item));
            }
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ControllerCategoryModel category)
        {
            ControllerCategoryModel? createdCategory = mapper.Map<ControllerCategoryModel>
                (categoryService.CreateCategory(ModelState.IsValid, mapper.Map<CategoryModel>(category)));
            if (createdCategory != null)
            {
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Edit(int id)
        {
            ControllerCategoryModel? category = mapper.Map<ControllerCategoryModel>(categoryService.GetCategory(id));

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ControllerCategoryModel category)
        {
            ControllerCategoryModel updatedCategory = mapper.Map<ControllerCategoryModel>(
                categoryService.UpdateCategory(ModelState.IsValid, mapper.Map<CategoryModel>(category)));
            if (updatedCategory != null)
            {
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public IActionResult Delete(int id)
        {
            ControllerCategoryModel? category = mapper.Map< ControllerCategoryModel>(categoryService.RemoveCategory(id));

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
