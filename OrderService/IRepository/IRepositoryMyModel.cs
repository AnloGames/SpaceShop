﻿using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.IRepository
{
    public interface IRepositoryMyModel : IRepository<MyModel>
    {
        void Update(MyModel obj);
    }
}
