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
    public class MyModelAdapter: IMyModelAdapter
    {
        readonly IRepositoryMyModel repositoryMyModel;
        readonly IMapper mapper;

        public MyModelAdapter(IRepositoryMyModel repositoryMyModel, IMapper mapper)
        {
            this.repositoryMyModel = repositoryMyModel;
            this.mapper = mapper;
        }

        public void Add(MyModelDto myModelDto)
        {
            MyModel myModel = mapper.Map<MyModel>(myModelDto);
            repositoryMyModel.Add(myModel);
        }

        public MyModelDto FirstOrDefaultById(int id, bool isTracking = false)
        {
            MyModel myModel = repositoryMyModel.FirstOrDefault(filter: x => x.Id == id, isTracking: isTracking);
            return mapper.Map<MyModelDto>(myModel);
        }

        public IEnumerable<MyModelDto> GetAll()
        {
            IEnumerable<MyModel> myModels = repositoryMyModel.GetAll();
            foreach (var model in myModels)
            {
                yield return mapper.Map<MyModelDto>(model);
            }
        }
        public IEnumerable<MyModelDto> GetAllByIdList(List<int> ids)
        {
            IEnumerable<MyModel> myModels = repositoryMyModel.GetAll(x => ids.Contains(x.Id));
            foreach (var model in myModels)
            {
                yield return mapper.Map<MyModelDto>(model);
            }
        }
        public void Remove(MyModelDto myModelDto)
        {
            MyModel myModel = mapper.Map<MyModel>(myModelDto);
            repositoryMyModel.Remove(myModel);
        }

        public void Save()
        {
            repositoryMyModel.Save();
        }

        public void Update(MyModelDto myModelDto)
        {
            MyModel myModel = mapper.Map<MyModel>(myModelDto);
            repositoryMyModel.Update(myModel);
        }
    }
}
