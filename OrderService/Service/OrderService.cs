using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LogicService.Service.IService;
using SpaceShop_Utility;
using LogicService.IRepository;
using SpaceShop_Utility.BrainTree;
using Microsoft.AspNetCore.Mvc;
using Braintree;
using Microsoft.AspNetCore.Http;
using SpaceShop_Models;

namespace LogicService.Service
{
    public class OrderService : IOrderService
    {
        IRepositoryOrderHeader repositoryOrderHeader;
        IRepositoryOrderDetail repositoryOrderDetail;
        IRepositoryProduct repositoryProduct;

        IBrainTreeBridge brainTreeBridge;


        public void SaveOrder(ApplicationUser user, List<Product> productList, IFormCollection collection)
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
                TransactionId = "NONE"
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


            string nonce = collection["payment_method_nonce"];

            var request = new TransactionRequest
            {
                Amount = 1,
                PaymentMethodNonce = nonce,
                OrderId = "1",
                Options = new TransactionOptionsRequest { SubmitForSettlement = true }
            };

            var getWay = brainTreeBridge.GetGateWay();

            var resultTransaction = getWay.Transaction.Sale(request);

            var id = resultTransaction.Target.Id;
            var status = resultTransaction.Target.ProcessorResponseText;

            orderHeader.TransactionId = id;
            repositoryOrderHeader.Save();
       
        }
    }
}
