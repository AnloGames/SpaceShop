using LogicService.Dto;
using LogicService.IAdapter;
using LogicService.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service
{
    public class MyModelService : IMyModelService
    {
        IMyModelAdapter myModelAdapter;
        public MyModelService(IMyModelAdapter myModelAdapter)
        {
            this.myModelAdapter = myModelAdapter;
        }

        public MyModelDto? CreateMyModel(bool isValid, MyModelDto myModel)
        {
            if (isValid)
            {
                myModelAdapter.Add(myModel);
                myModelAdapter.Save();
                return myModel;
            }
            return null;
        }

        public IEnumerable<MyModelDto> GetMyModels()
        {
            return myModelAdapter.GetAll();
        }

        public MyModelDto? RemoveMyModel(int id)
        {
            MyModelDto myModel = myModelAdapter.FirstOrDefaultById(id, isTracking: false);
            if (myModel == null)
            {
                return null;
            }
            myModelAdapter.Remove(myModel);
            myModelAdapter.Save();
            return myModel;
        }
    }
}
