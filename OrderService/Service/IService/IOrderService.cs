﻿using Microsoft.AspNetCore.Http;
using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service.IService
{
    public interface IOrderService
    {
        void SaveOrder(string UserId, ApplicationUser user, List<Product> porductList, string transactionId);
    }
}
    