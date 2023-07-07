using LogicService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.IAdapter
{
    public interface ICategoryAdapter
    {
        IEnumerable<CategoryDto> GetAll();
        CategoryDto Find(int id);
        CategoryDto FirstOrDefaultById(int id, bool isTracking = true);
        void Add(CategoryDto categoryDto);
        void Remove(CategoryDto categoryDto);
        void Update(CategoryDto categoryDto);
        void Save();
    }
}
