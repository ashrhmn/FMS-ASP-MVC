﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models.AdminEntities
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? FamilyId { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public int? Role { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public List<string> Emails { get; set; }
        public List<string> Phone { get; set; }
    }
}