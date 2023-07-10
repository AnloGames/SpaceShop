using LogicService.Dto;
using SpaceShop_Models;

namespace SpaceShop_ViewModels
{
    public class ProductCreation
    {
        public ProductDto product { get; set; }
        public IEnumerable<CategoryDto> categories { get; set; }
        public IEnumerable<MyModelDto> myModels { get; set; }
        public ProductCreation(ProductDto product, IEnumerable<CategoryDto> categories, IEnumerable<MyModelDto> myModels)
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
