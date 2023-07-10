using LogicService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.IAdapter
{
    public interface IConnectionProductMyModelAdapter
    {
        void Add(ConnectionProductMyModelDto connectionDto);
        void Remove(ConnectionProductMyModelDto connectionDto);
        void Update(ConnectionProductMyModelDto connectionDto);
        IEnumerable<ConnectionProductMyModelDto> GetAllByProductId(int id, bool isTracking = true, string includeProperties = null);
        IEnumerable<ConnectionProductMyModelDto> GetAllByMyModelId(int id, bool isTracking = true, string includeProperties = null);
        ConnectionProductMyModelDto FirstOrDefaultById(int id, bool isTracking = false);
        void Save();
    }
}
