using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShop_DataMigrations.Repository
{
    public class RepositoryCategory : Repository<Category>, IRepositoryCategory
    {
        public RepositoryCategory(ApplicationDbContext db) : base(db){}

        public void Update(Category obj)
        {
            var objFromDb = db.Category.FirstOrDefault(x => x.Id == obj.Id);

            if (objFromDb != null) 
            {
                objFromDb.DisplayOrder = obj.DisplayOrder;
                objFromDb.Name = obj.Name;
            }
        }
    }
}
