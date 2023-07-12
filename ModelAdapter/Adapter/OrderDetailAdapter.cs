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
    public class OrderDetailAdapter : IOrderDetailAdapter
    {
        readonly IRepositoryOrderDetail repositoryOrderDetail;
        readonly IMapper mapper;

        public OrderDetailAdapter(IMapper mapper, IRepositoryOrderDetail repositoryOrderDetail)
        {
            this.mapper = mapper;
            this.repositoryOrderDetail = repositoryOrderDetail;
        }


        public void Add(OrderDetailDto detailDto)
        {
            OrderDetail orderDetail = mapper.Map<OrderDetail>(detailDto);
            repositoryOrderDetail.Add(orderDetail);
        }

        public OrderDetailDto FirstOrDefaultById(int id)
        {
            OrderDetail detail = repositoryOrderDetail.FirstOrDefault(x => x.Id == id, isTracking: false);
            return mapper.Map<OrderDetailDto>(detail);
        }

        public IEnumerable<OrderDetailDto> GetAllByOrderHeaderId(int id, string includeProperties = null)
        {
            IEnumerable<OrderDetail> details = repositoryOrderDetail.GetAll(x => x.OrderHeaderId == id, isTracking: false, includeProperties: includeProperties);
            foreach (var detail in details)
            {
                yield return mapper.Map<OrderDetailDto>(detail);
            }
        }

        public void Remove(OrderDetailDto detailDto)
        {
            OrderDetail orderDetail = mapper.Map<OrderDetail>(detailDto);
            repositoryOrderDetail.Remove(orderDetail);
        }

        public void Save()
        {
            repositoryOrderDetail.Save();
        }

        public void Update(OrderDetailDto detailDto)
        {
            OrderDetail orderDetail = mapper.Map<OrderDetail>(detailDto);
            repositoryOrderDetail.Update(orderDetail);
        }
    }
}
