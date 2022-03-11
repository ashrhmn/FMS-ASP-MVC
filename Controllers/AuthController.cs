using Flight_Management_System.Auth;
using Flight_Management_System.Models;
using Flight_Management_System.Models.AuthEntities;
using Flight_Management_System.Models.Database;
using Flight_Management_System.Utils;
using JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flight_Management_System.Controllers
{
    public class AuthController : Controller
    {
        private Flight_ManagementEntities db;
        private JwtManage jwt;
        public AuthController()
        {
            db = new Flight_ManagementEntities();
            jwt = new JwtManage();
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            AuthPayload loggedInUser = jwt.LoggedInUser(Request.Cookies);

            if (loggedInUser == null) return View(new UserModel());

            switch (loggedInUser.Role)
            {
                case "admin":
                    // return redirect to admin dashboard when complete
                    return View(new UserModel());
                case "user":
                    // return redirect to user dashboard when complete
                    return View(new UserModel());
                case "flight_manager":
                    return RedirectToAction("Dashboard", "FlightManager");
                default:
                    return View(new UserModel());
            }
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            AuthPayload loggedInUser = jwt.LoggedInUser(Request.Cookies);

            if(loggedInUser==null) return View(new UserModel());

            switch (loggedInUser.Role)
            {
                case "admin":
                    // return redirect to admin dashboard when complete
                    return View(new UserModel());
                case "user":
                    return RedirectToAction("Dashboard", "User");
                case "flight_manager":
                    return RedirectToAction("Dashboard", "FlightManager");
                default:
                    return View(new UserModel());
            }
        }


        [HttpPost]
        public ActionResult SignUp(UserModel userModel)
        {
            var existingUser = db.Users.FirstOrDefault(u => u.Username == userModel.Username);
            if (existingUser == null)
            {
                
                if(ModelState.IsValid)
                {
                    var user = new User()
                {
                    Name = userModel.Name,
                    Username = userModel.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password, 12),
                    Address = userModel.Address,
                    DateOfBirth = userModel.DateOfBirth,
                    CityId = userModel.CityId,
                    FamilyId = userModel.FamilyId,
                    Email = userModel.Email,
                    Phone = userModel.Phone,
                    Role = 2
                    //Role = userModel.Role,
                };
                db.Users.Add(user);
                //db.SaveChanges();
                //var udata = GetUser(userModel.Username, userModel.Name);
                //var mail = new Email()
                //{
                //    Email1 = userModel.Mail,
                //    UserId = udata.Id
                //};
                //db.Emails.Add(mail);
                //db.SaveChanges();

                //var phn = new Phone()
                //{
                //    UserId = udata.Id,
                //    Phone1 = userModel.Cell
                //};
                //db.Phones.Add(phn);
                db.SaveChanges();

                return RedirectToAction("SignIn");
                //Ends
                }
                return View(userModel);
            }
            else
            {
                return null;
            }
        }


        [HttpPost]
        public ActionResult SignIn(UserModel userModel)
        {
            var user = db.Users.Where(u => u.Username == userModel.Username).First();

            bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(userModel.Password, user.Password);

            if (isCorrectPassword)
            {

                AuthPayload payload = new AuthPayload() 
                { 
                    Id = user.Id,
                    Username = user.Username,
                    Role=user.UserRoleEnum.Value
                };

                var token = jwt.EncodeToken(payload);
                HttpCookie cookie = new HttpCookie("session")
                {
                    Expires = DateTime.Now.AddDays(10)
                };
                cookie["token"] = token;
                Response.Cookies.Add(cookie);

                switch (payload.Role)
                {
                    case "admin":
                        // return redirect to admin dashboard when complete
                        return View(new UserModel());
                    case "user":
                        return RedirectToAction("Index", "User");
                    case "flight_manager":
                        return RedirectToAction("Dashboard", "FlightManager");
                    default:
                        return View(new UserModel());
                }
            }

            return RedirectToAction("SignIn");

        }

        [HttpGet]
        public string Token()
        {
            HttpCookie cookie = Request.Cookies["session"];
            return cookie["token"];
        }
        [HttpGet]
        public ActionResult Logout()
        {
            HttpCookie cookie = Request.Cookies["session"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public string CurrentUser()
        {
            AuthPayload user = jwt.LoggedInUser(Request.Cookies);
            return user.Username+" "+user.Role;
        }
    }
}
