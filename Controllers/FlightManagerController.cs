using Flight_Management_System.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flight_Management_System.Controllers
{
    public class FlightManagerController : Controller
    {
        [FlightManagerAccess]
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}