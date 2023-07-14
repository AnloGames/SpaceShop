using LogicService.Dto;
using LogicService.Models;

namespace LogicService.Dto.ViewModels
{
    public class ProductCreation
    {
        public ProductDto product { get; set; }
        public IEnumerable<CategoryModel> categories { get; set; }
        public IEnumerable<MyModelDto> myModels { get; set; }
        public ProductCreation(ProductDto product, IEnumerable<CategoryModel> categories, IEnumerable<MyModelDto> myModels)
        {
            this.product = product;
            this.categories = categories;
            this.myModels = myModels;
        }
        public ProductCreation(ProductDto product, IEnumerable<MyModelDto> myModels)
        {
            this.product = product;
            this.myModels = myModels;
        }
    }
}
