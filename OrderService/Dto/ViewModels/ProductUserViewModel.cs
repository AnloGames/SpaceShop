using LogicService.Dto;

namespace LogicService.Dto.ViewModels
{
    public class ProductUserViewModel
    {
        public ApplicationUserDto ApplicationUser { get; set; }
        public List<ProductDto> ProductList { get; set; }
    }
}
