using LogicService.IRepository;
using LogicService.Service.IService;
using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service
{
    public class ApplicationUserService : IApplicationUserService
    {
        IRepositoryApplicationUser repositoryApplicationUser;

        public ApplicationUserService(IRepositoryApplicationUser repositoryApplicationUser)
        {
            this.repositoryApplicationUser = repositoryApplicationUser;
        }

        public ApplicationUser GetApplicationUserByIdentity(ClaimsPrincipal User)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ApplicationUser applicationUser = repositoryApplicationUser.FirstOrDefault(x => x.Id == claim.Value);
            return applicationUser;
        }
    }
}
