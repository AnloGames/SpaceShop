using LogicService.Dto;

namespace LogicService.Dto.ViewModels
{
    public class OrderHeaderDetailViewModel
    {
        public OrderHeaderDto OrderHeader { get; set; }

        public IEnumerable<OrderDetailDto> OrderDetail { get; set; }
    }
}
