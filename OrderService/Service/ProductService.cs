using LogicService.IRepository;
using LogicService.Service.IService;
using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service
{
    public class ProductService : IProductService
    {
        IRepositoryProduct repositoryProduct;

        public ProductService(IRepositoryProduct repositoryProduct)
        {
            this.repositoryProduct = repositoryProduct;
        }   

        public IEnumerable<Product> GetProductsInCart(IEnumerable<Cart> cartList)
        {
            List<Product> productList = new List<Product>();
            foreach (var cart in cartList)
            {
                Product product = repositoryProduct.Find(cart.ProductId);
                product.TempCount = cart.TempCount;
                productList.Add(product);
            }
            return productList;
        }
    }
}
