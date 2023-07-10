using LogicService.Dto;
using SpaceShop_Models;

namespace SpaceShop_ViewModels.ViewModels
{
    public class ProductUserViewModel
    {
        public ApplicationUserDto ApplicationUser { get; set; }
        public List<ProductDto> ProductList { get; set; }
    }
}
