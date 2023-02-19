using SpaceShop_Models;
using System;

namespace SpaceShop_DataMigrations.Repository.IRepository
{
    public interface IRepositoryProduct : IRepository<Product>
    {
        void Update(Product obj);
    }
}
