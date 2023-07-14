using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SpaceShop.Models
{
    public class ControllerCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public int DisplayOrder { get; set; }
    }
}
