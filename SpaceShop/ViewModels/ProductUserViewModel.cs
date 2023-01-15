using SpaceShop.Models;

namespace SpaceShop.ViewModels
{
    public class ProductUserViewModel
    {
        public ApplicationUser applicationUser { get; set; }
        public List<Product> products { get; set; }
    }
}
