using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShop_DataMigrations.Repository.IRepository
{
    public interface IRepositoryOrderHeader : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
    }
}
