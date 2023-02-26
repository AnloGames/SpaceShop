using System;
using SpaceShop_Models;

namespace SpaceShop_DataMigrations.Repository.IRepository
{
    public interface IRepositoryQueryHeader : IRepository<QueryHeader>
    {
        void Update(QueryHeader obj);
    }
}