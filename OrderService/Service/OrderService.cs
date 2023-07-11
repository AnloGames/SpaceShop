using LogicService.Service.IService;
using SpaceShop_Utility;
using LogicService.IAdapter;
using LogicService.Dto;
using System.Security.Claims;
using LogicService.Dto.ViewModels;

namespace LogicService.Service
{
    public class OrderService : IOrderService
    {
        IOrderHeaderAdapter orderHeaderAdapter;
        IOrderDetailAdapter orderDetailAdapter;

        IProductAdapter productAdapter;

        public OrderService(IProductAdapter productAdapter, IOrderHeaderAdapter orderHeaderAdapter, IOrderDetailAdapter orderDetailAdapter)
        {
            this.productAdapter = productAdapter;
            this.orderHeaderAdapter = orderHeaderAdapter;
            this.orderDetailAdapter = orderDetailAdapter;
        }

        public void ChangeOrderStatus(string status, int orderHeaderId)
        {
            OrderHeaderDto orderHeader = orderHeaderAdapter.
                FirstOrDefaultById(orderHeaderId);

            orderHeader.Status = status;
            orderHeaderAdapter.Update(orderHeader);
            orderHeaderAdapter.Save();
        }

        public OrderHeaderDetailViewModel CreateOrderDetailViewModel(int orderHeaderId)
        {
            return new OrderHeaderDetailViewModel()
            {
                OrderHeader = orderHeaderAdapter.FirstOrDefaultById(orderHeaderId),
                OrderDetail = orderDetailAdapter.GetAllByOrderHeaderId(orderHeaderId, includeProperties: "Product")
            };
        }

        public IEnumerable<OrderHeaderDto> CreateOrderTable(ClaimsPrincipal User)
        {
            IEnumerable<OrderHeaderDto> orderHeaderList;
            if (User.IsInRole(PathManager.AdminRole))
            {
                orderHeaderList = orderHeaderAdapter.GetAll();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaderList = orderHeaderAdapter.GetAllByUserId(claim.Value);
            }
            return orderHeaderList;
        }

        public void ReturnProductInStock(int orderDetailId)
        {
            OrderDetailDto fullDetail = orderDetailAdapter.FirstOrDefaultById(orderDetailId);
            ProductDto product = productAdapter.FirstOrDefaultById(fullDetail.ProductId, isTracking: false);
            product.ShopCount += fullDetail.Count;
            fullDetail.IsProductHadReturn = true;

            productAdapter.Update(product);
            orderDetailAdapter.Update(fullDetail);
            productAdapter.Save();
        }

        public void SaveOrder(ApplicationUserDto user, List<ProductDto> productList, string transactionId)
        {
            int totalPrice = 0;

            foreach (var item in productList)
            {
                totalPrice += (int)(item.TempCount * item.Price);
            }
            OrderHeaderDto orderHeader = new OrderHeaderDto()
            {
                UserId = user.Id,
                DateOrder = DateTime.Now,
                TotalPrice = totalPrice,
                Status = PathManager.StatusAccepted,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                City = user.City,
                Street = user.Street,
                House = user.House,
                Apartment = user.Apartment,
                PostalCode = user.PostalCode,
                TransactionId = transactionId
};
            orderHeader = orderHeaderAdapter.AddAndChange(orderHeader);
            orderHeaderAdapter.Save();

            foreach (var product in productList)
            {
                OrderDetailDto orderDetail = new OrderDetailDto()
                {
                    OrderHeaderId = orderHeader.Id,
                    ProductId = product.Id,
                    Count = product.TempCount,
                    PricePerUnit = (int)product.Price,
                    IsProductHadReturn = false
                };
                ProductDto ShopProduct = productAdapter.FirstOrDefaultById(product.Id, isTracking: false);
                ShopProduct.ShopCount -= product.TempCount;
                productAdapter.Update(ShopProduct);
                orderDetailAdapter.Add(orderDetail);
            }

            orderDetailAdapter.Save();
       
        }
    }
}
