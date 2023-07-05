using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceShop_DataMigrations;
using SpaceShop_Models;
using SpaceShop_Utility;
using LogicService.IRepository;

namespace SpaceShop.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class MyModelController : Controller
    {
        private IRepositoryMyModel repositoryMyModel;

        public MyModelController(IRepositoryMyModel repositoryMyModel)
        {
            this.repositoryMyModel = repositoryMyModel;
        }
        public IActionResult Index()
        {
            IEnumerable<MyModel> MyModels = repositoryMyModel.GetAll();
            return View(MyModels);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MyModel myModel)
        {
            if (ModelState.IsValid)
            {
                repositoryMyModel.Add(myModel);
                repositoryMyModel.Save();
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

            MyModel myModel = repositoryMyModel.Find((int)id);

            if (myModel == null)
            {
                return NotFound();
            }

            repositoryMyModel.Remove(myModel);
            repositoryMyModel.Save();
            return RedirectToAction("Index");
        }
    }
}
