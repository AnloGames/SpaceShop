using LogicService.Dto;
using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service.IService
{
    public interface IProductService
    {
        IEnumerable<ProductDto> GetProductsInCart(IEnumerable<Cart> cartList);
    }
}
