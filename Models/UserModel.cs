using Flight_Management_System.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        public int? FamilyId { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int? CityId { get; set; }

        //public virtual City CityName { get; set; }
        public int? Role { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }

        [Required]
        public string Cell { get; set; }

        [Required]
        public string Mail { get; set; }

        [Required]
        [Compare("Password", ErrorMessage="Dosen't match with Password")]

        public string ConfirmPassword { get; set; }
        public List<string> Emails { get; set; }
        public List<string> Phone { get; set; }
    }
}