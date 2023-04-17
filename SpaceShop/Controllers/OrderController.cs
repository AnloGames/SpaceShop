using Microsoft.AspNetCore.Mvc;
using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_Utility.BrainTree;
using SpaceShop_Utility;
using SpaceShop_ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Braintree;
using SpaceShop_Models;

namespace SpaceShop.Controllers
{
    public class OrderController : Controller
    {
        [BindProperty]
        public OrderHeaderDetailViewModel OrderViewModel { get; set; }

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

        public IActionResult Index(string searchName = null, string searchEmail = null,
                    string searchPhone = null, string status = null)
        {
            OrderViewModel viewModel = new OrderViewModel()
            {
                OrderHeaderList = repositoryOrderHeader.GetAll(),
                StatusList = PathManager.StatusList.ToList().
                                Select(x => new SelectListItem { Text = x, Value = x })
};

            if (searchName != null)
            {
                viewModel.OrderHeaderList = viewModel.OrderHeaderList.
                    Where(x => x.FullName.ToLower().Contains(searchName.ToLower()));
            }

            if (searchEmail != null)
            {
                viewModel.OrderHeaderList = viewModel.OrderHeaderList.
                    Where(x => x.Email.ToLower().Contains(searchEmail.ToLower()));
            }

            if (searchPhone != null)
            {
                viewModel.OrderHeaderList = viewModel.OrderHeaderList.
                    Where(x => x.Phone.Contains(searchPhone));
            }

            if (status != null && status != "Choose Status")
            {
                viewModel.OrderHeaderList = viewModel.OrderHeaderList.
                    Where(x => x.Status.Contains(status));
            }

            return View(viewModel);
        }
        public IActionResult Details(int id)
        {
            OrderViewModel = new OrderHeaderDetailViewModel()
            {
                OrderHeader = repositoryOrderHeader.FirstOrDefault(x => x.Id == id),
                OrderDetail = repositoryOrderDetail.GetAll(x => x.OrderHeaderId == id, includeProperties: "Product")
            };


            return View(OrderViewModel);
        }
        [HttpPost]
        public IActionResult StartInProcessing()
        {
            // получаем объект из бд
            OrderHeader orderHeader = repositoryOrderHeader.
                FirstOrDefault(x => x.Id == OrderViewModel.OrderHeader.Id);

            orderHeader.Status = PathManager.StatusInProcess;

            repositoryOrderHeader.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult StartOrderDone()
        {
            OrderHeader orderHeader = repositoryOrderHeader.
    FirstOrDefault(x => x.Id == OrderViewModel.OrderHeader.Id);

            orderHeader.Status = PathManager.StatusAccepted;

            repositoryOrderHeader.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult StartOrderCancel()
        {
            OrderHeader orderHeader = repositoryOrderHeader.
     FirstOrDefault(x => x.Id == OrderViewModel.OrderHeader.Id);

            orderHeader.Status = PathManager.StatusDenied;

            repositoryOrderHeader.Save();

            return RedirectToAction("Index");
        }
    }
    
}
