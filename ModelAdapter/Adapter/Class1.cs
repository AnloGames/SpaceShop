using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicService.Dto;
using LogicService.IAdapter;

namespace ModelAdapter.Adapter
{
    class Class1 : Interface1
    {
        public List<CategoryDto> GetAllCategories()
        {
            return new List<CategoryDto>();
        }
    }
}
