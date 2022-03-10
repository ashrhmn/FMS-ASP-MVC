using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flight_Management_System.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class FlightManagerAccess : AuthorizeAttribute
    {
        private readonly Utils utils;
        public FlightManagerAccess()
        {
            utils = new Utils();
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return utils.Authenticate(httpContext, "flight_manager");
        }
    }
}