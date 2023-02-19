using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceShop_DataMigrations;
using SpaceShop_Models;
using SpaceShop_Utility;
using SpaceShop_DataMigrations.Repository.IRepository;

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
        public IActionResult Create(MyModel myModel)
        {
            repositoryMyModel.Add(myModel);
            repositoryMyModel.Save();

            return RedirectToAction("Index");
        }
    }
}
