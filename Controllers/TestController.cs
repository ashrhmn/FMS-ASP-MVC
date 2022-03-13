using Flight_Management_System.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flight_Management_System.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public JsonResult Index(string query)
        {
            var db = new Flight_ManagementEntities();
            //var users = db.Users.Where(ur => ur.Username.Contains(@"/" + query + "/")).ToList();
            var users = (from ur in db.Users where ur.Username.Contains(query) select ur).ToList();
            var u = users.Select(x => x.Username).ToList();
            return Json(u,JsonRequestBehavior.AllowGet);
        }
    }
}