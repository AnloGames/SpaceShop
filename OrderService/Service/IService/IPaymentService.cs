using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service.IService
{
    public interface IPaymentService
    {
        string GetTransactionId(IFormCollection collection);
        string GetTokenClient();
    }
}
