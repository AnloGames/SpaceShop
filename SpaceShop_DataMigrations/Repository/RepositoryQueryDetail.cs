using System;
using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_Models;

namespace SpaceShop_DataMigrations.Repository
{
    public class RepositoryQueryDetail : Repository<QueryDetail>, IRepositoryQueryDetail
    {
        public RepositoryQueryDetail(ApplicationDbContext db) : base(db) { }

        public void Update(QueryDetail obj)
        {
            db.QueryDetail.Update(obj);
        }
    }
}