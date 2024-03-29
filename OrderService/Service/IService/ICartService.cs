﻿using LogicService.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service.IService
{
    public interface ICartService
    {
        IEnumerable<Cart> GetCartListByProducts(IEnumerable<ProductDto> productList);
        IEnumerable<Cart> GetSessionCartList(HttpContext httpContext);
    }
}
