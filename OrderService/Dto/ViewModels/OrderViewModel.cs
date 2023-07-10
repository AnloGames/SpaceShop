using LogicService.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace LogicService.Dto.ViewModels
{
    public class OrderViewModel
    {
        public IEnumerable<OrderHeaderDto> OrderHeaderList { get; set; }

        // для выдападающего списка - статус
        public IEnumerable<SelectListItem> StatusList { get; set; }

        // текущее значение статуса
        public string Status { get; set; }
    }
}
