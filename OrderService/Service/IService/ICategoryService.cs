using LogicService.Dto;
using LogicService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service.IService
{
    public interface ICategoryService
    {
        IEnumerable<CategoryModel> GetAllCategories();
        CategoryModel? CreateCategory(bool isValid, CategoryModel category);
        CategoryModel? UpdateCategory(bool isValid, CategoryModel category);
        CategoryModel? GetCategory(int id);
        CategoryModel? RemoveCategory(int id);
    }
}
