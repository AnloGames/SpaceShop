using LogicService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.IAdapter
{
    public interface IOrderHeaderAdapter
    {
        void Add(OrderHeaderDto headerDto);
        OrderHeaderDto AddAndChange(OrderHeaderDto headerDto);
        void Update(OrderHeaderDto headerDto);
        void Remove(OrderHeaderDto headerDto);
        IEnumerable<OrderHeaderDto> GetAll();
        IEnumerable<OrderHeaderDto> GetAllByUserId(string id);
        OrderHeaderDto FirstOrDefaultById(int id);
        void Save();
    }
}
