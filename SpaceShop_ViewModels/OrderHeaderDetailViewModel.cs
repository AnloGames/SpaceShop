using LogicService.Dto;
using SpaceShop_Models;

namespace SpaceShop_ViewModels
{
    public class OrderHeaderDetailViewModel
    {
        public OrderHeaderDto OrderHeader { get; set; }

        public IEnumerable<OrderDetailDto> OrderDetail { get; set; }
    }
}
