using LogicService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service.IService
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDto> GetAllCategories();
        CategoryDto? CreateCategory(bool isValid, CategoryDto category);
        CategoryDto? UpdateCategory(bool isValid, CategoryDto category);
        CategoryDto? GetCategory(int id);
        CategoryDto? RemoveCategory(int id);
    }
}
