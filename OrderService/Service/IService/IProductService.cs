using LogicService.Dto;
using LogicService.Dto.ViewModels;
using Microsoft.AspNetCore.Http;
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
        ProductDto UploadImage(IFormFileCollection files, ProductDto product);
        ProductDto SetShopCount(ProductDto product, int count = -1);
        void RemoveProductMyModelConnections(int productId);
        ProductDto CompleteProductCreation(ProductDto product);
        void CreateProductMyModelConnections(ProductDto product, int[] myModelIds);
        void ChangeProductShortDescription(ProductDto product);
        ProductCreation CreateProductDeleteViewModel(int productId);
        void DeleteProduct(ProductDto product);  
    }
}
