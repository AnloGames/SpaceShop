using LogicService.Dto;
using SpaceShop_Models;

namespace SpaceShop_ViewModels
{
    public class ProductCreation
    {
        public ProductDto product { get; set; }
        public IEnumerable<CategoryDto> categories { get; set; }
        public IEnumerable<MyModel> myModels { get; set; }
        public ProductCreation(ProductDto product, IEnumerable<CategoryDto> categories, IEnumerable<MyModel> myModels)
        {
            this.product = product;
            this.categories = categories;
            this.myModels = myModels;
        }
        public ProductCreation(ProductDto product, IEnumerable<MyModel> myModels)
        {
            this.product = product;
            this.myModels = myModels;
        }
    }
}
