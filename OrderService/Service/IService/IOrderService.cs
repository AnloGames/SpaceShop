using LogicService.Dto;
using Microsoft.AspNetCore.Http;
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
        void SaveOrder(ApplicationUserDto user, List<ProductDto> porductList, string transactionId);
    }
}
    