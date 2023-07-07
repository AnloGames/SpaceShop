using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public double Price { get; set; }
        public string Image { get; set; }
        [Display(Name = "Category Id")]
        public int CategoryId { get; set; }
        public virtual CategoryDto Category { get; set; }
        public int ShopCount { get; set; }
        [Range(1, 100)]
        public int TempCount { get; set; } = 1;
    }
}
