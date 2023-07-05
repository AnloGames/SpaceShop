using SpaceShop_DataMigrations;
using SpaceShop_Models;
using LogicService.IRepository;

namespace SpaceShop_DataMigrations.Repository
{
    public class RepositoryProduct : Repository<Product>, IRepositoryProduct
    {
        public RepositoryProduct(ApplicationDbContext db) : base(db) { }

        public void Update(Product obj)
        {
            /*var objFromDb = db.Product.FirstOrDefault(x => x.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;    
                objFromDb.ShortDescription = obj.ShortDescription;
                objFromDb.Image = obj.Image;
                objFromDb.CategoryId = obj.CategoryId;
            }*/
            dbSet.Update(obj);
        }
    }
}
