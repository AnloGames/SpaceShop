
using LogicService.Service;
using LogicService.IAdapter;
using ModelAdapter.Adapter;
using Moq;
using System.Linq.Expressions;
using SpaceShop.Controllers;
using LogicService.Service.IService;
using LogicService.Dto;
using Microsoft.AspNetCore.Mvc;
using LogicService.Models;
using AutoMapper;

namespace UnitTest
{
    public class CategoryServiceTest
    {
        readonly Mock<ICategoryAdapter> mockCategoryAdapter = new Mock<ICategoryAdapter>();
        readonly Mock<IMapper> mockMapper = new Mock<IMapper>();
        CategoryService categoryService;

        List<CategoryDto> _categories;
        [SetUp]
        public void Setup()
        {
            mockCategoryAdapter.Setup(ca => ca.FirstOrDefaultById(It.IsAny<int>())).Returns((int id) => {
                if (id == 1 || id == 2)
                {
                    return new CategoryDto() { Id = id };
                }
                return null;
            });
            _categories = It.IsAny<List<CategoryDto>>();
            mockCategoryAdapter.Setup(ca => ca.GetAll()).Returns(_categories);
            categoryService = new CategoryService(mockCategoryAdapter.Object, mockMapper.Object);
        }

        [Test]
        public void GetAllCategoriesTest()
        {
            var res1 = categoryService.GetAllCategories();
            Assert.That(res1, Is.EqualTo(_categories));
        }

        [Test]
        public void CreateCategoryTest()
        {
            var res1 = categoryService.CreateCategory(false, new CategoryModel());
            Assert.IsNull(res1);
            var res2 = categoryService.CreateCategory(true, new CategoryModel());
            Assert.IsNotNull(res2);
        }
        [Test]
        public void RemoveCategoryTest()
        {
            int id = 1;
            var res1 = categoryService.RemoveCategory(id);
            var expectedRes = new CategoryDto() { Id = id };
            Assert.That(res1.Id, Is.EqualTo(expectedRes.Id));
            id = 3;
            var res2 = categoryService.RemoveCategory(id);
            Assert.IsNull(res2);
        }
        [Test]
        public void EditCategoryTest()
        {
            var res1 = categoryService.UpdateCategory(false, new CategoryModel());
            Assert.IsNull(res1);
            var res2 = categoryService.UpdateCategory(true, new CategoryModel());
            Assert.IsNotNull(res2);
        }
        [Test]
        public void GetCategoryTest()
        {
            int id = 1;
            var res1 = categoryService.GetCategory(id);
            var expectedRes = new CategoryDto() { Id = id };
            Assert.That(res1.Id, Is.EqualTo(expectedRes.Id));
            id = 3;
            var res2 = categoryService.GetCategory(id);
            Assert.IsNull(res2);
        }
    }
}