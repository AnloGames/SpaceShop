using SpaceShop_DataMigrations;
using SpaceShop_Models;
using SpaceShop_DataMigrations.Repository.IRepository;

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
