using SpaceShop_Models;

namespace LogicService.IRepository
{
    public interface IRepositoryCategory : IRepository<Category>
    {
        void Update(Category obj); 
    }
}
