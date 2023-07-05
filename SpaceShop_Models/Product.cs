using System.ComponentModel.DataAnnotations;
//using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceShop_Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public double Price { get; set; }

        public string Image { get; set; }

        [Display(Name = "Category Id")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public int ShopCount { get; set; }
        [NotMapped]
        [Range(1, 100)]
        public int TempCount { get; set; } = 1;
    }
}
