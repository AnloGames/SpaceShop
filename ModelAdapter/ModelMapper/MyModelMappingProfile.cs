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
    public class MyModelMappingProfile: Profile
    {
        public MyModelMappingProfile()
        {
            CreateMap<MyModel, MyModelDto>().ReverseMap();
        }
    }
}
