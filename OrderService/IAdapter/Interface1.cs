using LogicService.Dto;
using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.IAdapter
{
    public interface Interface1
    {
        List<CategoryDto> GetAllCategories();
    }
}
