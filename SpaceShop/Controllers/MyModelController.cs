using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceShop_DataMigrations;
using SpaceShop_Models;
using SpaceShop_Utility;
using LogicService.IAdapter;
using LogicService.Dto;

namespace SpaceShop.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class MyModelController : Controller
    {
        private IMyModelAdapter myModelAdapter;

        public MyModelController(IMyModelAdapter myModelAdapter)
        {
            this.myModelAdapter = myModelAdapter;
        }
        public IActionResult Index()
        {
            IEnumerable<MyModelDto> MyModels = myModelAdapter.GetAll();
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
            if (ModelState.IsValid)
            {
                myModelAdapter.Add(myModel);
                myModelAdapter.Save();
                return RedirectToAction("Index");
            }
            return View(myModel);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            MyModelDto myModel = myModelAdapter.FirstOrDefaultById((int)id, isTracking: false);

            if (myModel == null)
            {
                return NotFound();
            }

            myModelAdapter.Remove(myModel);
            myModelAdapter.Save();
            return RedirectToAction("Index");
        }
    }
}
