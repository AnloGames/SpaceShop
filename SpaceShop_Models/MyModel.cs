using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SpaceShop_Models
{
    public class MyModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Number { get; set; }
    }
}
