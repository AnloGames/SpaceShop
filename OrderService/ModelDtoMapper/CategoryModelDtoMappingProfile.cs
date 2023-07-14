using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LogicService.Dto;
using LogicService.Models;

namespace LogicService.ModelDtoMapper
{
    public class CategoryModelDtoMappingProfile : Profile
    {
        public CategoryModelDtoMappingProfile()
        {
            CreateMap<CategoryModel, CategoryDto>().ReverseMap();
        }
    }
}
