using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace SpaceShop_ViewModels
{
    public class OrderViewModel
    {
        public IEnumerable<OrderHeader> OrderHeaderList { get; set; }

        // для выдападающего списка - статус
        public IEnumerable<SelectListItem> StatusList { get; set; }

        // текущее значение статуса
        public string Status { get; set; }
    }
}
