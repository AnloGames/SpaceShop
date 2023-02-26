using System;
using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_Models;

namespace SpaceShop_DataMigrations.Repository
{
    public class RepositoryQueryHeader : Repository<QueryHeader>, IRepositoryQueryHeader
    {
        public RepositoryQueryHeader(ApplicationDbContext db) : base(db) { }

        public void Update(QueryHeader obj)
        {
            db.QueryHeader.Update(obj);
        }
    }
}