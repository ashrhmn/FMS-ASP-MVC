using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models
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
    }
}