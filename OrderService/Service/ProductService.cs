using LogicService.Dto;
using LogicService.Dto.ViewModels;
using LogicService.IAdapter;
using LogicService.Service.IService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SpaceShop_Utility;
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
        IWebHostEnvironment environment;
        IConnectionProductMyModelAdapter connectionProductMyModelAdapter;
        ICategoryAdapter categoryAdapter;
        IMyModelAdapter myModelAdapter;

        public ProductService(IProductAdapter productAdapter, IWebHostEnvironment environment,
            IConnectionProductMyModelAdapter connectionProductMyModelAdapter, ICategoryAdapter categoryAdapter, IMyModelAdapter myModelAdapter)
        {
            this.productAdapter = productAdapter;
            this.environment = environment;
            this.connectionProductMyModelAdapter = connectionProductMyModelAdapter;
            this.categoryAdapter = categoryAdapter;
            this.myModelAdapter = myModelAdapter;
        }

        public void ChangeProductShopCount(int productId, int count)
        {
            ProductDto product = productAdapter.FirstOrDefaultById(productId, isTracking: false);
            product.ShopCount = count;
            productAdapter.Update(product);
            productAdapter.Save();
        }

        public void ChangeProductShortDescription(ProductDto product)
        {
            CategoryDto productCategory = categoryAdapter.FirstOrDefaultById(product.CategoryId, isTracking: false);
            product.ShortDescription = "Category: " + productCategory.Name + "; Tags: ";
            IEnumerable<ConnectionProductMyModelDto> connections = connectionProductMyModelAdapter.GetAllByProductId(product.Id, isTracking: false);
            List<MyModelDto> myModels = new List<MyModelDto>();
            foreach (var connection in connections)
            {
                myModels.Add(myModelAdapter.FirstOrDefaultById(connection.MyModelId, isTracking: false));
            }
            foreach (var myModel in myModels)
            {
                product.ShortDescription += myModel.Name + ", ";
            }
            productAdapter.Update(product);
            productAdapter.Save();
        }

        public ProductDto CompleteProductCreation(ProductDto product)
        {
            if (product.Id == 0)
            {
                product = productAdapter.AddAndChange(product);
            }
            else
            {
                productAdapter.Update(product);
            }
            productAdapter.Save();
            return product;
        }

        public ProductCreation CreateProductDeleteViewModel(int productId)
        {
            ProductDto product = productAdapter.FirstOrDefaultById(productId);
            IEnumerable<ConnectionProductMyModelDto> connections = connectionProductMyModelAdapter.GetAllByProductId(productId);
            List<int> ids = new List<int>();
            foreach (var connection in connections)
            {
                ids.Add(connection.MyModelId);
            }
            IEnumerable<MyModelDto> connectedModels = myModelAdapter.GetAllByIdList(ids);
            ProductCreation productCreation = new ProductCreation(product, connectedModels);
            return productCreation;
        }

        public void CreateProductMyModelConnections(ProductDto product, int[] myModelIds)
        {
            foreach (int MyModelId in myModelIds)
            {
                ConnectionProductMyModelDto connection = new ConnectionProductMyModelDto();
                connection.Id = 0;
                connection.MyModelId = MyModelId;
                connection.ProductId = product.Id;
                connectionProductMyModelAdapter.Add(connection);
                connectionProductMyModelAdapter.Save();
            }
        }

        public void DeleteProduct(ProductDto product)
        {
            ProductDto NowProduct = productAdapter.FirstOrDefaultById(product.Id, isTracking: false);

            string wwwRoot = environment.WebRootPath;
            string upload = wwwRoot + PathManager.ImageProductPath;
            string oldFile = upload + NowProduct.Image;

            if (File.Exists(oldFile))
            {
                File.Delete(oldFile);
            }
            productAdapter.Remove(product);
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
                return productAdapter.GetAll(isTracking: false);
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

        public void RemoveProductMyModelConnections(int productId)
        {
            foreach (var connection in connectionProductMyModelAdapter.GetAllByProductId(productId, isTracking: false))
            {
                connectionProductMyModelAdapter.Remove(connection);
                productAdapter.Save();
            }
        }

        public ProductDto SetShopCount(ProductDto product, int count = -1)
        {
            if (count == -1)
            {
                ProductDto NowProduct = productAdapter.FirstOrDefaultById(product.Id, isTracking: false);
                product.ShopCount = NowProduct.ShopCount;
                return product;
            }
            product.ShopCount = count;
            return product;
        }

        public ProductDto UploadImage(IFormFileCollection files, ProductDto product)
        {
            string wwwRoot = environment.WebRootPath;
            if (product.Id == 0)
            {
                string upload = wwwRoot + PathManager.ImageProductPath;
                string imageName = Guid.NewGuid().ToString();

                if (files.Count > 0)
                {
                    string extension = Path.GetExtension(files[0].FileName);
                    string path = upload + imageName + extension;
                    using (var FileStream = new FileStream(path, FileMode.Create))
                    {
                        files[0].CopyTo(FileStream);
                    }
                    product.Image = imageName + extension;
                }
                else
                {
                    product.Image = "NONE.png";
                }

                product.ShopCount = 1;
            }
            else
            {
                ProductDto NowProduct = productAdapter.FirstOrDefaultById(product.Id, isTracking: false);
                product.ShopCount = NowProduct.ShopCount;
                if (files.Count > 0)
                {
                    string upload = wwwRoot + PathManager.ImageProductPath;
                    string imageName = Guid.NewGuid().ToString();

                    string extension = Path.GetExtension(files[0].FileName);
                    string path = upload + imageName + extension;
                    string oldFile = upload + NowProduct.Image;

                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }
                    using (var FileStream = new FileStream(path, FileMode.Create))
                    {
                        files[0].CopyTo(FileStream);
                    }

                    product.Image = imageName + extension;
                }
                else
                {
                    product.Image = NowProduct.Image;
                }
            }
            return product;
        }
    }
}
