using SpaceShop_Models;
using System;

namespace LogicService.IRepository
{
    public interface IRepositoryProduct : IRepository<Product>
    {
        void Update(Product obj);
    }
}
