using LogicService.Dto;
using LogicService.Dto.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service.IService
{
    public interface IHomeService
    {
        public HomeViewModel CreateHomeViewModel();
        public DetailsViewModel CreateDetailsViewModel(int productId, IEnumerable<Cart> cartList);
    }
}
