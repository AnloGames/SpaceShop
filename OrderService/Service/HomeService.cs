using LogicService.Dto;
using LogicService.Dto.ViewModels;
using LogicService.IAdapter;
using LogicService.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service
{
    public class HomeService : IHomeService
    {
        IProductAdapter productAdapter;
        ICategoryAdapter categoryAdapter;
        public HomeService(IProductAdapter productAdapter, ICategoryAdapter categoryAdapter)
        {
            this.productAdapter = productAdapter;
            this.categoryAdapter = categoryAdapter;
        }

        public DetailsViewModel CreateDetailsViewModel(int productId, IEnumerable<Cart> cartList)
        {
            DetailsViewModel detailsViewModel = new DetailsViewModel(false, productAdapter.FirstOrDefaultById(productId, includeProperties: "Category"));
            foreach (var item in cartList)
            {
                if (item.ProductId == productId)
                {
                    detailsViewModel.IsInCart = true;
                }
            }
            return detailsViewModel;
        }

        public HomeViewModel CreateHomeViewModel()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                products = productAdapter.GetAllByShopCount(0, includeProperties: "Category"),
                categories = categoryAdapter.GetAll()
            };
            return homeViewModel;
        }
    }
}
