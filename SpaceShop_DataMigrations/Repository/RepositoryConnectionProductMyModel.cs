using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShop_DataMigrations.Repository
{
    public class RepositoryConnectionProductMyModel : Repository<ConnectionProductMyModel>, IRepositoryConnectionProductMyModel
    {
        public RepositoryConnectionProductMyModel(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(ConnectionProductMyModel obj)
        {
            var objFromDb = db.ConnectionProductMyModel.FirstOrDefault(x => x.Id == obj.Id);

            if (objFromDb != null)
            {
                objFromDb.ProductId = obj.ProductId;
                objFromDb.MyModelId = obj.Id;
            }
        }
    }
}
