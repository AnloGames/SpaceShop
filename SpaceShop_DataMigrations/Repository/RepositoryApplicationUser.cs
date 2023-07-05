using SpaceShop_DataMigrations;
using SpaceShop_Models;
using LogicService.IRepository;

namespace SpaceShop_DataMigrations.Repository
{
    public class RepositoryApplicationUser : Repository<ApplicationUser>, IRepositoryApplicationUser
    {
        public RepositoryApplicationUser(ApplicationDbContext db) : base(db)
        {
        }
    }
}
