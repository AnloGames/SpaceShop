using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Dto
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public OrderHeaderDto OrderHeader { get; set; }
        public int ProductId { get; set; }
        public virtual ProductDto Product { get; set; }
        public int Count { get; set; }
        public int PricePerUnit { get; set; }
        public bool IsProductHadReturn { get; set; }
    }
}
