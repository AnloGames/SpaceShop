using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceShop_DataMigrations;
using SpaceShop_Models;
using SpaceShop_Utility;
using LogicService.IAdapter;
using LogicService.Dto;
using LogicService.Service;
using LogicService.Service.IService;

namespace SpaceShop.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class MyModelController : Controller
    {
        IMyModelService myModelService;
        public MyModelController(IMyModelService myModelService)
        {
            this.myModelService = myModelService;
        }
        public IActionResult Index()
        {
            IEnumerable<MyModelDto> MyModels = myModelService.GetMyModels();
            return View(MyModels);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MyModelDto myModel)
        {
            MyModelDto createdModel = myModelService.CreateMyModel(ModelState.IsValid, myModel);
            if (createdModel != null)
            {
                return RedirectToAction("Index");
            }
            return View(myModel);
        }

        public IActionResult Delete(int id)
        {
            MyModelDto myModel = myModelService.RemoveMyModel(id);

            if (myModel == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
    }
}
