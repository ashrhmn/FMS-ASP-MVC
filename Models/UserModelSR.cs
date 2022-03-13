﻿using Flight_Management_System.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models
{
    public class UserModelSR
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

        public int? Role { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}