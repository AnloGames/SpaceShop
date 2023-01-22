using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceShop.Data;
using SpaceShop.Models;
namespace SpaceShop.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class MyModelController : Controller
    {
        private ApplicationDbContext database;

        public MyModelController(ApplicationDbContext database)
        {
            this.database = database;
        }
        public IActionResult Index()
        {
            IEnumerable<MyModel> MyModels = database.MyModel;
            return View(MyModels);
        }
    }
}
