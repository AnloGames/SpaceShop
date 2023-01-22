using SpaceShop.Models;

namespace SpaceShop.ViewModels
{
    public class ProductUserViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<Product> ProductList { get; set; }
    }
}
