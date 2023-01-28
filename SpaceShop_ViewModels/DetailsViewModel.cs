using SpaceShop_Models;

namespace SpaceShop_ViewModels
{
    public class DetailsViewModel
    {
        public Product Product { get; set; }
        public bool IsInCart { get; set; }
        public DetailsViewModel(bool isInCart, Product product)
        {
            IsInCart = isInCart;
            Product = product;
        }
    }
}
