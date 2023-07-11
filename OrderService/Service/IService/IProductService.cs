using LogicService.Dto;
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
        int GetProductShopCount(int productId);
        ProductDto GetProduct(int productId);
        IEnumerable<ProductDto> GetProducts(int? categoryId);
        void ChangeProductShopCount(int productId, int count);
    }
}
