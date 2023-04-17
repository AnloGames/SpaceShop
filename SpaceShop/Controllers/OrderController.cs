using Microsoft.AspNetCore.Mvc;
using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_Utility.BrainTree;
using SpaceShop_Utility;
using SpaceShop_ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SpaceShop.Controllers
{
    public class OrderController : Controller
    {
        IRepositoryOrderHeader repositoryOrderHeader;
        IRepositoryOrderDetail repositoryOrderDetail;
        IBrainTreeBridge brainTreeBridge;

        public OrderController(IRepositoryOrderHeader repositoryOrderHeader,
            IRepositoryOrderDetail repositoryOrderDetail, IBrainTreeBridge brainTreeBridge)
        {
            this.brainTreeBridge = brainTreeBridge;
            this.repositoryOrderDetail = repositoryOrderDetail;
            this.repositoryOrderHeader = repositoryOrderHeader;
        }

        public IActionResult Index()
        {
            OrderViewModel viewModel = new OrderViewModel()
            {
                OrderHeaderList = repositoryOrderHeader.GetAll(),
                StatusList = PathManager.StatusList.ToList().
                                Select(x => new SelectListItem { Text = x, Value = x })
            };

            return View(viewModel);
        }
    }
    
}
