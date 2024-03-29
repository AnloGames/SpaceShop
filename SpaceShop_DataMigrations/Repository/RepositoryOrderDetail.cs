﻿using SpaceShop_DataMigrations;
using SpaceShop_Models;
using SpaceShop_DataMigrations.Repository.IRepository;

namespace SpaceShop_DataMigrations.Repository
{
    public class RepositoryOrderDetail : Repository<OrderDetail>, IRepositoryOrderDetail
    {
        public RepositoryOrderDetail(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(OrderDetail obj)
        {
            db.OrderDetail.Update(obj);
        }
    }
}
