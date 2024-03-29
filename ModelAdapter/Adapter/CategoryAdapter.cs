﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LogicService.Dto;
using LogicService.IAdapter;
using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_Models;

namespace ModelAdapter.Adapter
{
    public class CategoryAdapter : ICategoryAdapter
    {
        readonly IRepositoryCategory repositoryCategory;
        readonly IMapper mapper;

        public CategoryAdapter(IRepositoryCategory repositoryCategory, IMapper mapper)
        {
            this.repositoryCategory = repositoryCategory;
            this.mapper = mapper;
        }

        public void Add(CategoryDto categoryDto)
        {
            Category category = mapper.Map<Category>(categoryDto);
            repositoryCategory.Add(category);
        }

        public CategoryDto Find(int id)
        {
            Category category = repositoryCategory.Find(id);
            return mapper.Map<CategoryDto>(category);
        }

        public CategoryDto FirstOrDefaultById(int id)
        {
            Category category = repositoryCategory.FirstOrDefault(x => x.Id == id, isTracking: false);
            return mapper.Map<CategoryDto>(category);
        }

        public IEnumerable<CategoryDto> GetAll()
        {
            IEnumerable<Category> categories = repositoryCategory.GetAll();
            foreach (Category category in categories)
            {
                yield return mapper.Map<CategoryDto>(category);
            }
        }

        public void Remove(CategoryDto categoryDto)
        {
            Category category = mapper.Map<Category>(categoryDto);
            repositoryCategory.Remove(category);
        }

        public void Save()
        {
            repositoryCategory.Save();
        }

        public void Update(CategoryDto categoryDto)
        {
            Category category = mapper.Map<Category>(categoryDto);
            repositoryCategory.Update(category);
        }
    }
}
