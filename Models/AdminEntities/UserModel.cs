using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models.AdminEntities
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        public int? FamilyId { get; set; }
        [Required]
        public string Address { get; set; }
        public int? CityId { get; set; }
       // [Required]
        public int? Role { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public List<int> PurchasedTickets { get; set; }

        
        //[DataType(DataType.Password)]
        //[Required]
        public string OldPassword { get; set; }

        //[Required]
        public string ConPassword { get; set; }

    }
}