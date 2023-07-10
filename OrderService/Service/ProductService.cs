using LogicService.Dto;
using LogicService.IAdapter;
using LogicService.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service
{
    public class ProductService : IProductService
    {
        IProductAdapter productAdapter;

        public ProductService(IProductAdapter productAdapter)
        {
            this.productAdapter = productAdapter;
        }   

        public IEnumerable<ProductDto> GetProductsInCart(IEnumerable<Cart> cartList)
        {
            List<ProductDto> productList = new List<ProductDto>();
            foreach (var cart in cartList)
            {
                ProductDto product = productAdapter.Find(cart.ProductId);
                product.TempCount = cart.TempCount;
                productList.Add(product);
            }
            return productList;
        }
    }
}
