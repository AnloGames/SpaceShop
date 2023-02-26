using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceShop_Models
{
    public class QueryHeader
    {
        [Key]
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public DateTime QueryDate { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

    }
}