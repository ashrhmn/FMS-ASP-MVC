using Flight_Management_System.Models.AuthEntities;
using Flight_Management_System.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flight_Management_System.Auth
{
    public class Utils
    {
        private JwtManage jwt;
        public Utils()
        {
            jwt = new JwtManage();
        }
        public bool Authenticate(HttpContextBase httpContext, string role)
        {
            HttpCookie cookie = httpContext.Request.Cookies["session"];
            if (cookie == null) return false;
            var token = cookie["token"];
            if (token.Equals("")) return false;
            var decodedObject = jwt.DecodeToken(token);
            if(decodedObject == null) return false;
            AuthPayload result = JsonConvert.DeserializeObject<AuthPayload>(decodedObject);
            if (result.Role.Equals(role)) return true;
            return false;
        }
    }
}