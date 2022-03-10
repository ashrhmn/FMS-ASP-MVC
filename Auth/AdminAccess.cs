using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flight_Management_System.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AdminAccess : AuthorizeAttribute
    {
        private readonly Utils utils;
        public AdminAccess()
        {
            utils = new Utils();
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return utils.Authenticate(httpContext, "admin");
        }
    }
}