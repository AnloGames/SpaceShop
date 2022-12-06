using System;
using SpaceShop.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceShop.Models
{
    public class Plusor
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
        public int MyModelId { get; set; }

        [ForeignKey("MyModelId")]
        public virtual MyModel MyModel { get; set; }
    }
}
