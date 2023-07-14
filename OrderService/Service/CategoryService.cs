using AutoMapper;
using LogicService.Dto;
using LogicService.IAdapter;
using LogicService.Models;
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
        readonly ICategoryAdapter categoryAdapter;
        readonly IMapper mapper;    

        public CategoryService(ICategoryAdapter categoryAdapter, IMapper mapper)
        {
            this.categoryAdapter = categoryAdapter;
            this.mapper = mapper;
        }

        public CategoryModel? CreateCategory(bool isValid, CategoryModel category)
        {
            if (!isValid)
            {
                return null;
            }
            category.Id = 0;
            categoryAdapter.Add(mapper.Map<CategoryDto>(category));
            categoryAdapter.Save();
            return category;
        }

        public IEnumerable<CategoryModel> GetAllCategories()
        {
            foreach (var category in categoryAdapter.GetAll())
            {
                yield return mapper.Map<CategoryModel>(category);
            }

        }

        public CategoryModel? GetCategory(int id)
        {
            return mapper.Map<CategoryModel>(categoryAdapter.FirstOrDefaultById(id));
        }

        public CategoryModel? RemoveCategory(int id)
        {
            CategoryDto? category = categoryAdapter.FirstOrDefaultById(id);
            if (category == null) { return null; } 
            categoryAdapter.Remove(category);
            categoryAdapter.Save();
            return mapper.Map<CategoryModel>(category);
        }

        public CategoryModel? UpdateCategory(bool isValid, CategoryModel category)
        {
            if (!isValid)
            {
                return null;
            }
            categoryAdapter.Update(mapper.Map<CategoryDto>(category));
            categoryAdapter.Save();
            return category;
        }
    }
}
