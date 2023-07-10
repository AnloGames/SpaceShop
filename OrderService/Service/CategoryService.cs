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
    public class CategoryService : ICategoryService
    {
        private ICategoryAdapter categoryAdapter;

        public CategoryService(ICategoryAdapter categoryAdapter)
        {
            this.categoryAdapter = categoryAdapter;
        }

        public CategoryDto? CreateCategory(bool isValid, CategoryDto category)
        {
            if (!isValid)
            {
                return null;
            }
            category.Id = 0;
            categoryAdapter.Add(category);
            categoryAdapter.Save();
            return category;
        }

        public IEnumerable<CategoryDto> GetAllCategories()
        {
            throw new NotImplementedException();
        }

        public CategoryDto? GetCategory(int id)
        {
            return categoryAdapter.FirstOrDefaultById(id);
        }

        public CategoryDto? RemoveCategory(int id)
        {
            CategoryDto? category = categoryAdapter.FirstOrDefaultById(id);
            if (category.Id == 0) { return null; } 
            categoryAdapter.Remove(category);
            categoryAdapter.Save();
            return category;
        }

        public CategoryDto? UpdateCategory(bool isValid, CategoryDto category)
        {
            if (!isValid)
            {
                return null;
            }
            categoryAdapter.Update(category);
            categoryAdapter.Save();
            return category;
        }
    }
}
