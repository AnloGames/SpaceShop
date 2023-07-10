using LogicService.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpaceShop_Models;


namespace SpaceShop_ViewModels
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
