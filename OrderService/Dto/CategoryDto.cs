using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public int DisplayOrder { get; set; }
    }
}
