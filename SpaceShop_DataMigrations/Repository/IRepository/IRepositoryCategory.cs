using SpaceShop_Models;

namespace SpaceShop_DataMigrations.Repository.IRepository
{
    public interface IRepositoryCategory : IRepository<Category>
    {
        void Update(Category obj); 
    }
}
