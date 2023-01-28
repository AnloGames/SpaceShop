using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceShop_Models
{
    public class ConnectionProductMyModel
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public int MyModelId { get; set; }

        [ForeignKey("MyModelId")]
        public virtual MyModel MyModel { get; set; }
    }
}
