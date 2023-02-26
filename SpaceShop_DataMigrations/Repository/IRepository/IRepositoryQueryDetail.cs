using System;
using SpaceShop_Models;

namespace SpaceShop_DataMigrations.Repository.IRepository
{
    public interface IRepositoryQueryDetail : IRepository<QueryDetail>
    {
        void Update(QueryDetail obj);
    }
}