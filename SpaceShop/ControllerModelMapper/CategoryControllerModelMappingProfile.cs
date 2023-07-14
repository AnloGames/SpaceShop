using AutoMapper;
using LogicService.Models;
using SpaceShop.Models;

namespace SpaceShop.ControllerModelMapper
{
    public class CategoryControllerModelMappingProfile: Profile
    {
        public CategoryControllerModelMappingProfile()
        {
            CreateMap<ControllerCategoryModel, CategoryModel>().ReverseMap();
        }
    }
}
