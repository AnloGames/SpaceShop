using System;
using SpaceShop.Models;
namespace SpaceShop.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> products { get; set; }
        public IEnumerable<Category> categories { get; set; }
    }
}
