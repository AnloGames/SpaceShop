using LogicService.Dto;
using SpaceShop_Models;
using AutoMapper;

namespace ModelAdapter.ModelMapper
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
