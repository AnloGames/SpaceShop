using LogicService.Service.IService;
using SpaceShop_Utility;
using LogicService.IRepository;
using SpaceShop_Models;
using LogicService.IAdapter;
using LogicService.Dto;

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
