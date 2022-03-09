using Flight_Management_System.Models.AdminEntities;
using Flight_Management_System.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flight_Management_System.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details()
        {
            Flight_ManagementEntities db = new Flight_ManagementEntities();
            var data = (from a in db.Users where a.Username.Equals("Ashik") select a).FirstOrDefault();
            var user = new UserModel();
            user.Username = data.Username;
            user.DateOfBirth = data.DateOfBirth;
            //user.CityName = data.City==null?"Undefined":data.City.Name;
            //user.CountryName = data.City == null ? "Undefined" : data.City.Country;
            user.Address= data.Address;
            user.Emails = data.Emails.Select(e => e.Email1).ToList();
            user.Phone = data.Phones.Select(e => e.Phone1).ToList();

            return View(user);
        }
        [HttpGet]
        public ActionResult EditProfile()
        {
            Flight_ManagementEntities db = new Flight_ManagementEntities();
            var data = (from a in db.Users where a.Username.Equals("Ashik") select a).FirstOrDefault();

            var user = new UserModel();
            user.Username = data.Username;
            user.Password = data.Password;
            user.DateOfBirth = data.DateOfBirth;
            //user.CityName = data.City==null?"Undefined":data.City.Name;
            //user.CountryName = data.City == null ? "Undefined" : data.City.Country;
            user.Address = data.Address;
            user.Emails = data.Emails.Select(e => e.Email1).ToList();
            user.Phone = data.Phones.Select(e => e.Phone1).ToList();
            return View(user);
        }
        [HttpPost]
        public ActionResult EditProfile(UserModel user)
        {
            if (ModelState.IsValid)
            {
                Flight_ManagementEntities db = new Flight_ManagementEntities();
                var data = (from a in db.Users where a.Username.Equals("Ashik") select a).FirstOrDefault();
                return RedirectToAction("Details");
            }
            return View();
            
        }
        [HttpGet]
        public ActionResult ChangePass()
        {
            //Flight_ManagementEntities db = new Flight_ManagementEntities();
            //var data = (from a in db.Users where a.Username.Equals("Ashik") select a.Password).FirstOrDefault();
            //var user = new UserModel();
            //user.Password = data;
            return View();

        }
        [HttpPost]
        public ActionResult ChangePass(string OldPassword, string Password, string ConPassword)
        {
            if (Password.Equals(ConPassword))
            {
                Flight_ManagementEntities db = new Flight_ManagementEntities();
                var data = (from a in db.Users where a.Username.Equals("Ashik") select a).FirstOrDefault();

                bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(OldPassword, data.Password);

                if (isCorrectPassword)
                {
                    var user = new UserModel()
                    {
                        Name = data.Name,
                        Username = data.Username,
                        Password = BCrypt.Net.BCrypt.HashPassword(Password, 12),
                        Address = data.Address,
                        DateOfBirth = data.DateOfBirth,
                        CityId = data.CityId,
                        FamilyId = data.FamilyId,
                        Role = data.Role,
                    };
                    db.Entry(data).CurrentValues.SetValues(user);
                    db.SaveChanges();
                    TempData["msg"] = "Password Changed Successfully";
                    return RedirectToAction("Dtails");
                }
                TempData["msg"] = "Old Password is not correct";
                return View();

            }
            TempData["msg"] = "Password & Confirm Password Not Matched";
            return View();

        }
    }
}