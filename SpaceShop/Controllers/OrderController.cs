using Microsoft.AspNetCore.Mvc;
using LogicService.IRepository;
using SpaceShop_Utility.BrainTree;
using SpaceShop_Utility;
using SpaceShop_ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Braintree;
using SpaceShop_Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LogicService.IAdapter;
using LogicService.Dto;

namespace SpaceShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        [BindProperty]
        public OrderHeaderDetailViewModel OrderViewModel { get; set; }

        IRepositoryOrderHeader repositoryOrderHeader;
        IRepositoryOrderDetail repositoryOrderDetail;
        IBrainTreeBridge brainTreeBridge;

        IProductAdapter productAdapter;

        public OrderController(IRepositoryOrderHeader repositoryOrderHeader,
            IRepositoryOrderDetail repositoryOrderDetail, IBrainTreeBridge brainTreeBridge,
            IProductAdapter productAdapter)
        {
            this.brainTreeBridge = brainTreeBridge;
            this.repositoryOrderDetail = repositoryOrderDetail;
            this.repositoryOrderHeader = repositoryOrderHeader;
            this.productAdapter = productAdapter;
        }

        public IActionResult Index(string searchName = null, string searchEmail = null,
                    string searchPhone = null, string status = null)
        {
            IEnumerable<OrderHeader> orderHeaderList;
            if (User.IsInRole(PathManager.AdminRole))
            {
                orderHeaderList = repositoryOrderHeader.GetAll();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaderList = repositoryOrderHeader.GetAll(x => x.UserId == claim.Value);
            }
            OrderViewModel viewModel = new OrderViewModel()
            {
                OrderHeaderList = orderHeaderList,
                StatusList = PathManager.StatusList.ToList().
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
            OrderViewModel = new OrderHeaderDetailViewModel()
            {
                OrderHeader = repositoryOrderHeader.FirstOrDefault(x => x.Id == id),
                OrderDetail = repositoryOrderDetail.GetAll(x => x.OrderHeaderId == id, includeProperties: "Product")
            };


            return View(OrderViewModel);
        }
        [Authorize(Roles = PathManager.AdminRole)]
        public IActionResult ReturnInStock(int id)
        {
            OrderDetail fullDetail = repositoryOrderDetail.Find(id);
            ProductDto product = productAdapter.FirstOrDefaultById(fullDetail.ProductId,isTracking: false);
            product.ShopCount += fullDetail.Count;
            fullDetail.IsProductHadReturn = true;

            productAdapter.Update(product);
            repositoryOrderDetail.Update(fullDetail);
            productAdapter.Save();


            return RedirectToAction("Details", "Order", new {id = fullDetail.OrderHeaderId});
        }
        [Authorize(Roles = PathManager.AdminRole)]
        [HttpPost]
        public IActionResult StartInProcessing()
        {
            // получаем объект из бд
            OrderHeader orderHeader = repositoryOrderHeader.
                FirstOrDefault(x => x.Id == OrderViewModel.OrderHeader.Id);

            orderHeader.Status = PathManager.StatusInProcess;

            repositoryOrderHeader.Save();

            return RedirectToAction("Details", "Order", new { id = orderHeader.Id });
        }
        [Authorize(Roles = PathManager.AdminRole)]
        [HttpPost]
        public IActionResult StartOrderDone()
        {
            OrderHeader orderHeader = repositoryOrderHeader.
                FirstOrDefault(x => x.Id == OrderViewModel.OrderHeader.Id);

            orderHeader.Status = PathManager.StatusOrderDone;
            repositoryOrderHeader.Save();

            return RedirectToAction("Details", "Order", new { id = orderHeader.Id });
        }
        [Authorize(Roles = PathManager.AdminRole)]
        [HttpPost]
        public IActionResult StartOrderCancel()
        {
            OrderHeader orderHeader = repositoryOrderHeader.
                FirstOrDefault(x => x.Id == OrderViewModel.OrderHeader.Id);


            var gateWay = brainTreeBridge.GetGateWay();

            // get transaction
            Transaction transaction = gateWay.Transaction.Find(orderHeader.TransactionId);

            // условия при которых не возвращаем
            if (transaction.Status == TransactionStatus.AUTHORIZED ||
                transaction.Status == TransactionStatus.SUBMITTED_FOR_SETTLEMENT)
            {
                var res = gateWay.Transaction.Void(orderHeader.TransactionId);
            }
            else // возврат средств
            {
                var res = gateWay.Transaction.Refund(orderHeader.TransactionId);
            }

            orderHeader.Status = PathManager.StatusDenied;
            repositoryOrderHeader.Save();

            return RedirectToAction("Details", "Order", new { id = orderHeader.Id });
        }
    }
    
}
