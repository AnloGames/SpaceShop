﻿using LogicService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service.IService
{
    public interface IMyModelService
    {
        IEnumerable<MyModelDto> GetMyModels();
        MyModelDto? RemoveMyModel(int id);
        MyModelDto? CreateMyModel(bool isValid, MyModelDto myModel);
    }
}