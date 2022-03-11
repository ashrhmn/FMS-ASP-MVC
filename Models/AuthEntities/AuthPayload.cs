using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models.AuthEntities
{
    public class AuthPayload
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}