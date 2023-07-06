using Microsoft.AspNetCore.Http;
using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service.IService
{
    public interface ICartService
    {
        IEnumerable<Cart> GetCartListByProducts(IEnumerable<Product> productList);
        IEnumerable<Cart> GetSessionCartList(HttpContext httpContext);
    }
}
