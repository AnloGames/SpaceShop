using LogicService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.IAdapter
{
    public interface IApplicationUserAdapter
    {
        void Add(ApplicationUserDto userDto);
        void Remove(ApplicationUserDto userDto);
        void Save();
        ApplicationUserDto FirstOrDefaultById(string id, bool isTracking = true);
    }
}
