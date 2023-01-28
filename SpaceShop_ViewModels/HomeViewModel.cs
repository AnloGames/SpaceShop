using System;
using SpaceShop_Models;

namespace SpaceShop_ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> products { get; set; }
        public IEnumerable<Category> categories { get; set; }
    }
}
