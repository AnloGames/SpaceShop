using LogicService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.IAdapter
{
    public interface IOrderDetailAdapter
    {
        void Add(OrderDetailDto detailDto);
        void Update(OrderDetailDto detailDto);
        void Remove(OrderDetailDto detailDto);
        OrderDetailDto FirstOrDefaultById(int id);
        IEnumerable<OrderDetailDto> GetAllByOrderHeaderId(int id, string includeProperties = null);
        void Save();

    }
}
