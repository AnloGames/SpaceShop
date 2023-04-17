using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceShop_Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        [NotMapped]
        public string City { get; set; }
        [NotMapped]
        public string Street { get; set; }
        [NotMapped]
        public string House { get; set; }
        [NotMapped]
        public string Apartment { get; set; }
        [NotMapped]
        public string PostalCode { get; set; }
    }
        //Если хотим добавить в таблицу столбец, наследуем класс от модели таблицы
}
