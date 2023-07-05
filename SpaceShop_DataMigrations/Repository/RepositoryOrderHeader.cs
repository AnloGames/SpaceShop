﻿using SpaceShop_DataMigrations;
using SpaceShop_Models;
using LogicService.IRepository;

namespace SpaceShop_DataMigrations.Repository
{
    public class RepositoryOrderHeader : Repository<OrderHeader>, IRepositoryOrderHeader
    {
        public RepositoryOrderHeader(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(OrderHeader obj)
        {
            db.OrderHeader.Update(obj);
        }
    }
}
