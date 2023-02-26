using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceShop_Models
{
    public class QueryDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QueryHeaderId { get; set; }
        [ForeignKey("QueryHeaderId")]
        public virtual QueryHeader QueryHeader { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
