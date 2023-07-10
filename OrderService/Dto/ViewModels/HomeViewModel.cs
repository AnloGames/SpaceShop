using LogicService.Dto;

namespace LogicService.Dto.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ProductDto> products { get; set; }
        public IEnumerable<CategoryDto> categories { get; set; }
    }
}
