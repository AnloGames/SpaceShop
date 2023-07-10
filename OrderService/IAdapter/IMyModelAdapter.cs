using LogicService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.IAdapter
{
    public interface IMyModelAdapter
    {
        IEnumerable<MyModelDto> GetAll();
        IEnumerable<MyModelDto> GetAllByIdList(List<int> ids);
        MyModelDto FirstOrDefaultById(int id, bool isTracking = true);
        void Add(MyModelDto myModelDto);
        void Remove(MyModelDto myModelDto);
        void Update(MyModelDto myModelDto);
        void Save();
    }
}
