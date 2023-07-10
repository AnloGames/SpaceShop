using SpaceShop_DataMigrations;
using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_Models;

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
