using AutoMapper;
using LogicService.Dto;
using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAdapter.ModelMapper
{
    public class ApplicationUserMappingProfile : Profile
    {
        public ApplicationUserMappingProfile()
        {
            CreateMap<ApplicationUserDto, ApplicationUser>().ReverseMap();
        }
    }
}
