using Microsoft.AspNetCore.Mvc;
using SpaceShop_DataMigrations.Repository.IRepository;

namespace SpaceShop.Controllers
{
    public class QueryController : Controller
    {
        private IRepositoryQueryHeader repositoryQueryHeader;
        private IRepositoryQueryDetail repositoryQueryDetail;

        public QueryController(IRepositoryQueryHeader repositoryQueryHeader, IRepositoryQueryDetail repositoryQueryDetail)
        {
            this.repositoryQueryHeader = repositoryQueryHeader;
            this.repositoryQueryDetail = repositoryQueryDetail;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
