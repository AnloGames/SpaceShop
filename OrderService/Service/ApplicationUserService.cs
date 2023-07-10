using LogicService.Dto;
using LogicService.IAdapter;
using LogicService.Service.IService;
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
        IApplicationUserAdapter applicationUserAdapter;

        public ApplicationUserService(IApplicationUserAdapter applicationUserAdapter)
        {
            this.applicationUserAdapter = applicationUserAdapter;
        }

        public ApplicationUserDto GetApplicationUserByIdentity(ClaimsPrincipal User)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ApplicationUserDto applicationUser = applicationUserAdapter.FirstOrDefaultById(claim.Value);
            return applicationUser;
        }
    }
}
