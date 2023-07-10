using AutoMapper;
using LogicService.Dto;
using LogicService.IAdapter;
using LogicService.IRepository;
using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAdapter.Adapter
{
    public class ApplicationUserAdapter : IApplicationUserAdapter
    {
        public IRepositoryApplicationUser repositoryApplicationUser;
        public IMapper mapper;

        public ApplicationUserAdapter(IRepositoryApplicationUser repositoryApplicationUser, IMapper mapper)
        {
            this.repositoryApplicationUser = repositoryApplicationUser;
            this.mapper = mapper;
        }

        public void Add(ApplicationUserDto userDto)
        {
            ApplicationUser user = mapper.Map<ApplicationUser>(userDto);
            repositoryApplicationUser.Add(user);
        }

        public ApplicationUserDto FirstOrDefaultById(string id, bool isTracking = true)
        {
            ApplicationUser user = repositoryApplicationUser.FirstOrDefault(x => x.Id == id, isTracking: isTracking);
            return mapper.Map<ApplicationUserDto>(user);
        }

        public void Remove(ApplicationUserDto userDto)
        {
            ApplicationUser user = mapper.Map<ApplicationUser>(userDto);
            repositoryApplicationUser.Remove(user);
        }

        public void Save()
        {
            repositoryApplicationUser.Save();
        }
    }
}
