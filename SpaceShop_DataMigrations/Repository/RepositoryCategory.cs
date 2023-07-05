using SpaceShop_DataMigrations;
using LogicService.IRepository;
using SpaceShop_Models;

namespace SpaceShop_DataMigrations.Repository
{
    public class RepositoryCategory : Repository<Category>, IRepositoryCategory
    {
        public RepositoryCategory(ApplicationDbContext db) : base(db) { }

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
