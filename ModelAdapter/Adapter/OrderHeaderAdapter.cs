using AutoMapper;
using LogicService.Dto;
using LogicService.IAdapter;
using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAdapter.Adapter
{
    public class OrderHeaderAdapter : IOrderHeaderAdapter
    {
        readonly IMapper mapper;
        readonly IRepositoryOrderHeader repositoryOrderHeader;
        public OrderHeaderAdapter(IMapper mapper, IRepositoryOrderHeader repositoryOrderHeader)
        {
            this.mapper = mapper;
            this.repositoryOrderHeader = repositoryOrderHeader;
        }

        public void Add(OrderHeaderDto headerDto)
        {
            OrderHeader header = mapper.Map<OrderHeader>(headerDto);
            repositoryOrderHeader.Add(header);
        }

        public OrderHeaderDto AddAndChange(OrderHeaderDto headerDto)
        {
            OrderHeader header = mapper.Map<OrderHeader>(headerDto);
            repositoryOrderHeader.Add(header);
            repositoryOrderHeader.Save();
            return mapper.Map<OrderHeaderDto>(header);
        }

        public OrderHeaderDto FirstOrDefaultById(int id)
        {
            OrderHeader header = repositoryOrderHeader.FirstOrDefault(x => x.Id == id, isTracking: false);
            return mapper.Map<OrderHeaderDto>(header);
        }

        public IEnumerable<OrderHeaderDto> GetAll()
        {
            IEnumerable<OrderHeader> headers = repositoryOrderHeader.GetAll();
            foreach (var header in headers)
            {
                yield return mapper.Map<OrderHeaderDto>(header);
            }
        }

        public IEnumerable<OrderHeaderDto> GetAllByUserId(string id)
        {
            IEnumerable<OrderHeader> headers = repositoryOrderHeader.GetAll(x => x.UserId == id);
            foreach (var header in headers)
            {
                yield return mapper.Map<OrderHeaderDto>(header);
            }
        }

        public void Remove(OrderHeaderDto headerDto)
        {
            OrderHeader header = mapper.Map<OrderHeader>(headerDto);
            repositoryOrderHeader.Remove(header);
        }

        public void Save()
        {
            repositoryOrderHeader.Save();
        }

        public void Update(OrderHeaderDto headerDto)
        {
            OrderHeader header = mapper.Map<OrderHeader>(headerDto);
            repositoryOrderHeader.Update(header);
        }
    }
}
