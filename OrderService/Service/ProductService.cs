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

        public void ChangeProductShopCount(int productId, int count)
        {
            ProductDto product = productAdapter.FirstOrDefaultById(id, isTracking: false);
            product.ShopCount = count;
            productAdapter.Update(product);
            productAdapter.Save();
        }

        public ProductDto GetProduct(int productId)
        {
            return productAdapter.FirstOrDefaultById(productId, isTracking: false);
        }

        public IEnumerable<ProductDto> GetProducts(int? categoryId)
        {
            if (categoryId == null)
            {
                return productAdapter.GetAll();
            }
            else
            {
                return productAdapter.GetAllByCategoryId((int)categoryId, isTracking: false);

            }
        }

        public int GetProductShopCount(int productId)
        {
            ProductDto product = productAdapter.Find(productId);
            return product.ShopCount;
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
