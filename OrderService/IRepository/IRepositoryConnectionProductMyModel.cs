﻿using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.IRepository
{
    public interface IRepositoryConnectionProductMyModel : IRepository<ConnectionProductMyModel>
    {
        void Update(ConnectionProductMyModel obj);
    }
}