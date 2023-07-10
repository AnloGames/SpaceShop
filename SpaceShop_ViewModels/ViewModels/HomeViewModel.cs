using LogicService.Dto;
using SpaceShop_Models;

namespace SpaceShop_ViewModels.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ProductDto> products { get; set; }
        public IEnumerable<CategoryDto> categories { get; set; }
    }
}
