using LogicService.Dto;
using LogicService.Service.IService;
using Microsoft.AspNetCore.Http;
using SpaceShop_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service
{
    public class CartService : ICartService
    {
        public IEnumerable<Cart> GetCartListByProducts(IEnumerable<ProductDto> productList)
        {
            List<Cart> cartList = new List<Cart>();
            foreach (var prod in productList)
            {
                cartList.Add(new Cart()
                {
                    ProductId = prod.Id,
                    TempCount = prod.TempCount
                });
            }
            return cartList;
        }

        public IEnumerable<Cart> GetSessionCartList(HttpContext httpContext)
        {
            IEnumerable<Cart> cartList = new List<Cart>();
            if (httpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
            && httpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Any())
            {
                cartList = httpContext.Session.Get<List<Cart>>(PathManager.SessionCart);
            }
            return cartList;
        }
    }
}
