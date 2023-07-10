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
    public class ConnectionProductMyModelAdapter : IConnectionProductMyModelAdapter
    {
        IRepositoryConnectionProductMyModel repositoryConnectionProductMyModel;
        IMapper mapper;

        public ConnectionProductMyModelAdapter(IRepositoryConnectionProductMyModel repositoryConnectionProductMyModel, IMapper mapper)
        {
            this.repositoryConnectionProductMyModel = repositoryConnectionProductMyModel;
            this.mapper = mapper;
        }

        public void Add(ConnectionProductMyModelDto connectionDto)
        {
            ConnectionProductMyModel connection = mapper.Map<ConnectionProductMyModel>(connectionDto);
            repositoryConnectionProductMyModel.Add(connection);
        }

        public ConnectionProductMyModelDto FirstOrDefaultById(int id, bool isTracking = false)
        {
            ConnectionProductMyModel connection = repositoryConnectionProductMyModel.FirstOrDefault(x => x.Id == id, isTracking: isTracking);
            return mapper.Map<ConnectionProductMyModelDto>(connection);
        }

        public IEnumerable<ConnectionProductMyModelDto> GetAllByMyModelId(int id, bool isTracking = true, string includeProperties = null)
        {
            IEnumerable<ConnectionProductMyModel> connections = repositoryConnectionProductMyModel.GetAll(x => x.MyModelId == id, isTracking: isTracking, includeProperties: includeProperties);
            foreach (var connection in connections)
            {
                yield return mapper.Map<ConnectionProductMyModelDto>(connection);
            }
        }

        public IEnumerable<ConnectionProductMyModelDto> GetAllByProductId(int id, bool isTracking = true, string includeProperties = null)
        {
            IEnumerable<ConnectionProductMyModel> connections = repositoryConnectionProductMyModel.GetAll(x => x.ProductId == id, isTracking: isTracking, includeProperties: includeProperties);
            foreach (var connection in connections)
            {
                yield return mapper.Map<ConnectionProductMyModelDto>(connection);
            }
        }

        public void Remove(ConnectionProductMyModelDto connectionDto)
        {
            ConnectionProductMyModel connection = mapper.Map<ConnectionProductMyModel>(connectionDto);
            repositoryConnectionProductMyModel.Remove(connection);
        }

        public void Save()
        {
            repositoryConnectionProductMyModel.Save();
        }

        public void Update(ConnectionProductMyModelDto connectionDto)
        {
            ConnectionProductMyModel connection = mapper.Map<ConnectionProductMyModel>(connectionDto);
            repositoryConnectionProductMyModel.Update(connection);
        }
    }
}
