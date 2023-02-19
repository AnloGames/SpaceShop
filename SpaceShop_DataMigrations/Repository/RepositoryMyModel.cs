using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_Models;

namespace SpaceShop_DataMigrations.Repository
{
    public class RepositoryMyModel : Repository<MyModel>, IRepositoryMyModel
    {
        public RepositoryMyModel(ApplicationDbContext db) : base(db) { }
        public void Update(MyModel obj)
        {
            var objFromDb = db.MyModel.FirstOrDefault(x => x.Id == obj.Id);

            if (objFromDb != null)
            {
                objFromDb.Number = obj.Number;
                objFromDb.Name = obj.Name;
            }
        }
    }
}
