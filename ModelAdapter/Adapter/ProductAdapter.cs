using AutoMapper;
using LogicService.Dto;
using LogicService.IAdapter;
using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceShop_DataMigrations.Repository.IRepository;
using System.Threading.Tasks;

namespace ModelAdapter.Adapter
{
    public class ProductAdapter : IProductAdapter
    {
        readonly IRepositoryProduct repositoryProduct;
        readonly IMapper mapper;

        public ProductAdapter(IRepositoryProduct repositoryProduct, IMapper mapper)
        {
            this.repositoryProduct = repositoryProduct;
            this.mapper = mapper;
        }
        public void Add(ProductDto productDto)
        {
            Product product = mapper.Map<Product>(productDto);
            repositoryProduct.Add(product);
        }

        public ProductDto AddAndChange(ProductDto productDto)
        {
            Product product = mapper.Map<Product>(productDto);
            repositoryProduct.Add(product);
            repositoryProduct.Save();
            return mapper.Map<ProductDto>(product);
        }

        public ProductDto Find(int id)
        {
            Product product = repositoryProduct.Find(id);
            return mapper.Map<ProductDto>(product);
        }

        public ProductDto FirstOrDefaultById(int id, bool isTracking = false, string includeProperties = null)
        {
            Product product = repositoryProduct.FirstOrDefault(filter: x => x.Id == id, isTracking: isTracking, includeProperties: includeProperties);
            return mapper.Map<ProductDto>(product);
        }

        public IEnumerable<ProductDto> GetAll(bool isTracking = false, string includeProperties = null)
        {
            IEnumerable<Product> products = repositoryProduct.GetAll(isTracking: isTracking, includeProperties: includeProperties);
            foreach (Product product in products)
            {
                yield return mapper.Map<ProductDto>(product);
            }
        }

        public IEnumerable<ProductDto> GetAllByCategoryId(int categoryId, bool isTracking = false, string includeProperties = null)
        {
            IEnumerable<Product> products = repositoryProduct.GetAll(filter: x => x.CategoryId == categoryId, isTracking: isTracking, includeProperties: includeProperties);
            foreach (Product product in products)
            {
                yield return mapper.Map<ProductDto>(product);
            }
        }

        public IEnumerable<ProductDto> GetAllByShopCount(int preferredShopCount, bool isTracking = false, string includeProperties = null)
        {
            IEnumerable<Product> products = repositoryProduct.GetAll(filter: x => x.ShopCount > preferredShopCount, isTracking: isTracking, includeProperties: includeProperties);
            foreach (Product product in products)
            {
                yield return mapper.Map<ProductDto>(product);
            }
        }

        public void Remove(ProductDto productDto)
        {
            Product product = mapper.Map<Product>(productDto);
            repositoryProduct.Remove(product);
        }

        public void Save()
        {
            repositoryProduct.Save();
        }

        public void Update(ProductDto productDto)
        {
            Product product = mapper.Map<Product>(productDto);
            repositoryProduct.Update(product);
        }
    }
}
