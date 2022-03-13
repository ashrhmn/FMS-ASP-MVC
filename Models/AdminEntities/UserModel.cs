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

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? FamilyId { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public int? Role { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<int> PurchasedTickets { get; set; }

        [Required]
        //[DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConPassword { get; set; }

    }
}