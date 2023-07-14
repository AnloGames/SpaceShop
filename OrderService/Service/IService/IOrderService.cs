using LogicService.Dto;
using LogicService.Dto.ViewModels;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Service.IService
{
    public interface IOrderService
    {
        void SaveOrder(ApplicationUserDto user, List<ProductDto> porductList, string transactionId);
        IEnumerable<OrderHeaderDto> CreateOrderTable(ClaimsPrincipal User);
        OrderHeaderDetailViewModel CreateOrderDetailViewModel(int orderHeaderId);
        OrderDetailDto ReturnProductInStock(int orderDetailId);
        void ChangeOrderStatus(string status, int orderHeaderId);
    }
}
    