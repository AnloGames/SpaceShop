using Microsoft.AspNetCore.Mvc;
using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_Utility.BrainTree;
using SpaceShop_Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Braintree;
using SpaceShop_Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LogicService.IAdapter;
using LogicService.Dto;
using LogicService.Dto.ViewModels;
using LogicService.Service.IService;

namespace SpaceShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        [BindProperty]
        public OrderHeaderDetailViewModel OrderViewModel { get; set; }

        readonly IOrderService orderService;
        readonly IPaymentService paymentService;

        public OrderController(IOrderService orderService, IPaymentService paymentService)
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
        }

        public IActionResult Index(string searchName = null, string searchEmail = null,
                    string searchPhone = null, string status = null)
        {
            IEnumerable<OrderHeaderDto> orderHeaderList = orderService.CreateOrderTable(User);
            OrderViewModel viewModel = new OrderViewModel()
            {
                OrderHeaderList = orderHeaderList,
                StatusList = PathManager.StatusList.
                                Select(x => new SelectListItem { Text = x, Value = x })
};
            viewModel.OrderHeaderList = viewModel.OrderHeaderList.Reverse();

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
            OrderViewModel = orderService.CreateOrderDetailViewModel(id);
            return View(OrderViewModel);
        }
        [Authorize(Roles = PathManager.AdminRole)]
        public IActionResult ReturnInStock(int id)
        {
            orderService.ReturnProductInStock(id);
            return RedirectToAction("Details", "Order", new {id = OrderViewModel.OrderHeader.Id});
        }
        [Authorize(Roles = PathManager.AdminRole)]
        [HttpPost]
        public IActionResult StartInProcessing()
        {
            orderService.ChangeOrderStatus(PathManager.StatusInProcess, OrderViewModel.OrderHeader.Id);
            return RedirectToAction("Details", "Order", new { id = OrderViewModel.OrderHeader.Id });
        }
        [Authorize(Roles = PathManager.AdminRole)]
        [HttpPost]
        public IActionResult StartOrderDone()
        {
            orderService.ChangeOrderStatus(PathManager.StatusOrderDone, OrderViewModel.OrderHeader.Id);
            return RedirectToAction("Details", "Order", new { id = OrderViewModel.OrderHeader.Id });
        }
        [Authorize(Roles = PathManager.AdminRole)]
        [HttpPost]
        public IActionResult StartOrderCancel()
        {
            OrderHeaderDto orderHeader = OrderViewModel.OrderHeader;
            orderService.ChangeOrderStatus(PathManager.StatusDenied, orderHeader.Id);
            paymentService.RefundTransaction(orderHeader.TransactionId);

            return RedirectToAction("Details", "Order", new { id = orderHeader.Id });
        }
    }
    
}
