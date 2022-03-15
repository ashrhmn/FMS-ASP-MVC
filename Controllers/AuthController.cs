
using Flight_Management_System.Models;
using Flight_Management_System.Models.AuthEntities;
using Flight_Management_System.Models.Database;
using Flight_Management_System.Utils;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flight_Management_System.Controllers
{
    public class AuthController : Controller
    {
        private readonly Flight_ManagementEntities _db;
        private readonly JwtManage _jwt;
        public AuthController()
        {
            _db = new Flight_ManagementEntities();
            _jwt = new JwtManage();
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            AuthPayload loggedInUser = _jwt.LoggedInUser(Request.Cookies);

            if (loggedInUser == null) return View(new UserSignUpModel());

            switch (loggedInUser.Role)
            {
                case "admin":
                    return RedirectToAction("Index", "Admin");
                case "user":
                    return RedirectToAction("Dashboard", "User");
                case "flight_manager":
                    return RedirectToAction("Dashboard", "FlightManager");
                default:
                    return View(new UserSignUpModel());
            }
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            AuthPayload loggedInUser = _jwt.LoggedInUser(Request.Cookies);

            if (loggedInUser == null) return View(new UserLoginModel());

            switch (loggedInUser.Role)
            {
                case "admin":
                    return RedirectToAction("Index", "Admin");
                case "user":
                    return RedirectToAction("Dashboard", "User");
                case "flight_manager":
                    return RedirectToAction("Dashboard", "FlightManager");
                default:
                    return View(new UserLoginModel());
            }
        }


        [HttpPost]
        public ActionResult SignUp(UserSignUpModel userModel)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.Username == userModel.Username);
            if (existingUser != null)
            {
                TempData["msg"] = "Username already in use";
                return View(userModel);
            }
            if (!ModelState.IsValid) return View(userModel);
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
            };
            _db.Users.Add(user);
            _db.SaveChanges();

            return RedirectToAction("SignIn");
        }


        [HttpPost]
        public ActionResult SignIn(UserLoginModel userModel)
        {
            if (!ModelState.IsValid) return View(userModel);
            var user = _db.Users.FirstOrDefault(u => u.Username.Equals(userModel.Username));

            if (user == null) {
                TempData["msg"] = "User does not exist";
                return View(userModel);
            }

            bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(userModel.Password, user.Password);

            switch (isCorrectPassword)
            {
                case true:
                {
                    var payload = new AuthPayload()
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Role = user.UserRoleEnum.Value
                    };

                    var token = _jwt.EncodeToken(payload);
                    var cookie = new HttpCookie("session")
                    {
                        Expires = DateTime.Now.AddDays(10),
                        ["token"] = token
                    };
                    Response.Cookies.Add(cookie);
                    break;
                }
                case false:
                    TempData["msg"] = "Username or password is incorrect";
                    return View(userModel);
            }

            return RedirectToAction("SignIn");

        }

        [HttpGet]
        public string Token()
        {
            var cookie = Request.Cookies["session"];
            return cookie?["token"];
        }
        [HttpGet]
        public ActionResult Logout()
        {
            var cookie = Request.Cookies["session"];
            if (cookie == null) return RedirectToAction("SignIn");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public string CurrentUser()
        {
            AuthPayload user = _jwt.LoggedInUser(Request.Cookies);
            return user.Username + " " + user.Role;
        }
    }
}
