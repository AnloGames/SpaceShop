using SpaceShop_Models;

namespace SpaceShop_ViewModels
{
    public class ProductCreation
    {
        public Product product { get; set; }
        public IEnumerable<Category> categories { get; set; }
        public IEnumerable<MyModel> myModels { get; set; }
        public ProductCreation(Product product, IEnumerable<Category> categories, IEnumerable<MyModel> myModels)
        {
            this.product = product;
            this.categories = categories;
            this.myModels = myModels;
        }
        public ProductCreation(Product product, IEnumerable<MyModel> myModels)
        {
            this.product = product;
            this.myModels = myModels;
        }
    }
}
