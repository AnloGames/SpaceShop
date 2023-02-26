using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShop_DataMigrations.Repository
{
    public class RepositoryApplicationUser : Repository<ApplicationUser>, IRepositoryApplicationUser
    {
        public RepositoryApplicationUser(ApplicationDbContext db) : base(db)
        {
        }
    }
}
