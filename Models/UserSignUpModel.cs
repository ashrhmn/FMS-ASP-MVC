using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models
{
    public class UserSignUpModel
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-z][a-z0-9]{4,10}$", ErrorMessage = "Must be all lowercase latter and at least 4 charaecter long and maximum 10 charecter.")]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$",
            ErrorMessage = "Password must be of Minimum eight characters and contains at least one uppercase letter, one lowercase letter, one number")]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Z]{4,20}$", ErrorMessage = "Must Start With Capital Latter and can't have number and space and at least 4 charaecter long and maximum 20 charecter.")]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        public int? FamilyId { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int? CityId { get; set; }

        public int? Role { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }

        [Required]
        [RegularExpression(@"(^(\+88|0088)?(01){1}[3456789]{1}(\d){8})$", ErrorMessage = "Not a Valid Phone Number.")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}