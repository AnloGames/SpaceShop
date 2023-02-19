using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShop_DataMigrations.Repository.IRepository
{
    public interface IRepositoryConnectionProductMyModel : IRepository<ConnectionProductMyModel>
    {
        void Update(ConnectionProductMyModel obj);
    }
}
