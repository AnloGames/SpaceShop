using LogicService.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.IAdapter
{
    public interface IProductAdapter
    {
        ProductDto Find(int id);
        IEnumerable<ProductDto> GetAll(bool isTracking = true, string includeProperties = null);
        IEnumerable<ProductDto> GetAllByShopCount(int preferredShopCount, bool isTracking = true, string includeProperties = null);
        IEnumerable<ProductDto> GetAllByCategoryId(int categoryId, bool isTracking = true, string includeProperties = null);
        ProductDto FirstOrDefaultById(int id, bool isTracking = false, string includeProperties = null);
        void Add(ProductDto productDto);
        ProductDto AddAndChange(ProductDto productDto);
        void Update(ProductDto productDto);
        void Remove(ProductDto productDto);
        void Save();
    }
}
