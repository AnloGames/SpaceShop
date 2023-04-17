using Microsoft.AspNetCore.Mvc;
using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_Models;
using SpaceShop_ViewModels;
using SpaceShop_Utility;
using Microsoft.AspNetCore.Authorization;

namespace SpaceShop.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class QueryController : Controller
    {
        private IRepositoryQueryHeader repositoryQueryHeader;
        private IRepositoryQueryDetail repositoryQueryDetail;

        [BindProperty]
        public QueryViewModel QueryViewModel { get; set; }

        public QueryController(IRepositoryQueryHeader repositoryQueryHeader, IRepositoryQueryDetail repositoryQueryDetail)
        {
            this.repositoryQueryHeader = repositoryQueryHeader;
            this.repositoryQueryDetail = repositoryQueryDetail;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            QueryViewModel = new QueryViewModel()
            {
                QueryHeader = repositoryQueryHeader.FirstOrDefault(x => x.Id == id),
                QueryDetails = repositoryQueryDetail.GetAll(x => x.QueryHeaderId == id, includeProperties: "Product")
            }; 
            return View(QueryViewModel);
        }
        [HttpPost]
        public IActionResult Details()
        {
            List<Cart> carts = new List<Cart>(); 

            QueryViewModel.QueryDetails = repositoryQueryDetail.GetAll(x => x.QueryHeaderId == QueryViewModel.QueryHeader.Id);
            foreach (var item in QueryViewModel.QueryDetails)
            {
                Cart cart = new Cart() { ProductId = item.ProductId};

                carts.Add(cart);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(PathManager.SessionCart, carts);

            HttpContext.Session.Set(PathManager.SessionQuery, QueryViewModel.QueryHeader.Id);
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult Delete()
        {
            QueryHeader queryHeader = repositoryQueryHeader.FirstOrDefault(x => x.Id == QueryViewModel.QueryHeader.Id);

            IEnumerable<QueryDetail> details = repositoryQueryDetail.GetAll(x => x.QueryHeaderId == QueryViewModel.QueryHeader.Id);

            repositoryQueryHeader.Remove(queryHeader);
            repositoryQueryDetail.Remove(details);

            repositoryQueryHeader.Save();

            return RedirectToAction("Index");
        }

        public IActionResult GetQueryList()
        {
            JsonResult result = Json(new { data = repositoryQueryHeader.GetAll() });
            return result;
        }
    }

}
