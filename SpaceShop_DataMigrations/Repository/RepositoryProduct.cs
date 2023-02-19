using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShop_DataMigrations.Repository
{
    public class RepositoryProduct : Repository<Product>, IRepositoryProduct
    {
        public RepositoryProduct(ApplicationDbContext db) : base(db) { }

        public void Update(Product obj)
        {
            var objFromDb = db.Product.FirstOrDefault(x => x.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;    
                objFromDb.ShortDescription = obj.ShortDescription;
                objFromDb.Image = obj.Image;
                objFromDb.CategoryId = obj.CategoryId;
            }
        }
    }
}
