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
        // GET: Auth
        public ActionResult Index()
        {
            return View();
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
                    // return redirect to f_manager dashboard when complete
                    return View(new UserModel());
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
                    // return redirect to user dashboard when complete
                    return View(new UserModel());
                case "flight_manager":
                    // return redirect to f_manager dashboard when complete
                    return View(new UserModel());
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
                var user = new User()
                {
                    Name = userModel.Name,
                    Username = userModel.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password, 12),
                    Address = userModel.Address,
                    DateOfBirth = userModel.DateOfBirth,
                    CityId = userModel.CityId,
                    FamilyId = userModel.FamilyId,
                    Role = 2
                    //Role = userModel.Role,
                };
                db.Users.Add(user);
                db.SaveChanges();
                var udata = GetUser(userModel.Username, userModel.Name);
                var mail = new Email()
                {
                    Email1 = userModel.Mail,
                    UserId = udata.Id
                };
                db.Emails.Add(mail);
                db.SaveChanges();

                var phn = new Phone()
                {
                    UserId = udata.Id,
                    Phone1 = userModel.Cell
                };
                db.Phones.Add(phn);
                db.SaveChanges();

                return RedirectToAction("SignIn");
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
            }

            return RedirectToAction("SignUp");

        }

        [HttpGet]
        public string Token()
        {
            HttpCookie cookie = Request.Cookies["session"];
            return cookie["token"];
        }

        [UserAccess]
        [HttpGet]
        public string CurrentUser()
        {
            AuthPayload user = jwt.LoggedInUser(Request.Cookies);
            return user.Username+user.Role;
        }

        // GET: Auth/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Auth/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Auth/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Auth/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Auth/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Auth/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public User GetUser(string uname, string name)
        {
            var data = (from u in db.Users
                        where u.Username.Equals(uname) && u.Name.Equals(name)
                        select u).FirstOrDefault();
            return data;
        }
    }
}
