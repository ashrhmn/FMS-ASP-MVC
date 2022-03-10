using Flight_Management_System.Models.AuthEntities;
using Flight_Management_System.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flight_Management_System.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UserAccess : AuthorizeAttribute
    {
        private readonly Utils utils;
        public UserAccess()
        {
            utils = new Utils();
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return utils.Authenticate(httpContext, "user");
        }
    }
}