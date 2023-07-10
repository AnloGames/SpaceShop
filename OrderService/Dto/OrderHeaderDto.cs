using SpaceShop_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicService.Dto
{
    public class OrderHeaderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUserDto User { get; set; }
        public DateTime DateOrder { get; set; }
        public int TotalPrice { get; set; }
        public string Status { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }
        public string PostalCode { get; set; }
        public string TransactionId { get; set; }

        public DateTime DateExecution { get; set; }
    }
}
