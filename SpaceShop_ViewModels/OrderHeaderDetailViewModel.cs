using SpaceShop_Models;

namespace SpaceShop_ViewModels
{
    public class OrderHeaderDetailViewModel
    {
        public OrderHeader OrderHeader { get; set; }

        public IEnumerable<OrderDetail> OrderDetail { get; set; }
    }
}
