using LogicService.Service.IService;
using SpaceShop_Utility;
using LogicService.IRepository;
using SpaceShop_Models;

namespace LogicService.Service
{
    public class OrderService : IOrderService
    {
        IRepositoryOrderHeader repositoryOrderHeader;
        IRepositoryOrderDetail repositoryOrderDetail;
        IRepositoryProduct repositoryProduct;

        public OrderService(IRepositoryOrderHeader repositoryOrderHeader, IRepositoryOrderDetail repositoryOrderDetail, IRepositoryProduct repositoryProduct)
        {
            this.repositoryOrderHeader = repositoryOrderHeader;
            this.repositoryOrderDetail = repositoryOrderDetail;
            this.repositoryProduct = repositoryProduct;
        }

        public void SaveOrder(ApplicationUser user, List<Product> productList, string transactionId)
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
                Product ShopProduct = repositoryProduct.Find(product.Id);
                ShopProduct.ShopCount -= product.TempCount;
                repositoryProduct.Update(ShopProduct);
                repositoryOrderDetail.Add(orderDetail);
            }

            repositoryOrderDetail.Save();
       
        }
    }
}
