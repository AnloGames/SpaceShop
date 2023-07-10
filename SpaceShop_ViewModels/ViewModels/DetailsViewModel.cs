using SpaceShop_Models;
using LogicService.Dto;

namespace SpaceShop_ViewModels.ViewModels
{
    public class DetailsViewModel
    {
        public ProductDto Product { get; set; }
        public bool IsInCart { get; set; }
        public DetailsViewModel(bool isInCart, ProductDto product)
        {
            IsInCart = isInCart;
            Product = product;
        }
    }
}
