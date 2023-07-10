using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Dto
{
    public class ConnectionProductMyModelDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual ProductDto Product { get; set; }
        public int MyModelId { get; set; }
        public virtual MyModelDto MyModel { get; set; }
    }
}
