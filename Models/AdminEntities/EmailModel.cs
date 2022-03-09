using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Models.AdminEntities
{
    public class EmailModel
    {
        public int Id { get; set; }
        public string Email1 { get; set; }
        public int UserId { get; set; }
    }
}