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
        IRepositoryOrderHeader repositoryOrderHeader;
        IRepositoryOrderDetail repositoryOrderDetail;

        IProductAdapter productAdapter;

        public OrderService(IRepositoryOrderHeader repositoryOrderHeader, IRepositoryOrderDetail repositoryOrderDetail, IProductAdapter productAdapter)
        {
            this.repositoryOrderHeader = repositoryOrderHeader;
            this.repositoryOrderDetail = repositoryOrderDetail;
            this.productAdapter = productAdapter;
        }

        public void SaveOrder(ApplicationUser user, List<ProductDto> productList, string transactionId)
        {
            int totalPrice = 0;

            foreach (var item in productList)
            {
                totalPrice += (int)(item.TempCount * item.Price);
            }
            OrderHeader orderHeader = new OrderHeader()
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
            repositoryOrderHeader.Add(orderHeader);
            repositoryOrderHeader.Save();

            foreach (var product in productList)
            {
                OrderDetail orderDetail = new OrderDetail()
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
                repositoryOrderDetail.Add(orderDetail);
            }

            repositoryOrderDetail.Save();
       
        }
    }
}
