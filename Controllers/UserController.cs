﻿using Flight_Management_System.Auth;
using Flight_Management_System.Models;
using Flight_Management_System.Models.AuthEntities;
using Flight_Management_System.Models.Database;
using Flight_Management_System.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flight_Management_System.Controllers
{
    [UserAccess]
    public class UserController : Controller
    {
        private Flight_ManagementEntities db;
        private JwtManage jwt;
        public UserController()
        {
            db = new Flight_ManagementEntities();
            jwt = new JwtManage();
        }
        // GET: User
        public ActionResult Index()
        {
            AuthPayload user = jwt.LoggedInUser(Request.Cookies);
            int uid = user.Id;
            var udata = GetUser(uid);
            return View(udata);
        }

        [HttpGet]
        public ActionResult Flights()
        {

            var data = (from t in db.Transports
                        select t).ToList();
            return View(data);
        }

        //[HttpPost]
        //public ActionResult Flights(Transport flMod)
        //{
        //    var data = (from t in db.Transports
        //                where t.FromStopageId == flMod.FromStopageId && t.ToStopageId == flMod.ToStopageId
        //                select t).ToList();
        //    return View(data);
        //}

        [HttpGet]
        public ActionResult Dashboard()
        {
            AuthPayload user = jwt.LoggedInUser(Request.Cookies);
            int uid = user.Id;
            UserModel userModel = new UserModel();
            var udata = GetUser(uid);
            userModel.Id = udata.Id;
            userModel.Name = udata.Name;
            userModel.Phone = udata.Phone;
            userModel.Email = udata.Email;
            userModel.Address = udata.Address;
            userModel.CityId = udata.CityId;
            userModel.CityName = udata.CityId==null?"Undefined": udata.City.Name;
            if (udata.DateOfBirth.HasValue) { userModel.DateOfBirth = udata.DateOfBirth.Value; }
            userModel.Username = udata.Username;
            userModel.Password = udata.Password;
            return View(userModel);
        }
        
        [HttpPost]
        public ActionResult Dashboard(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                AuthPayload user = jwt.LoggedInUser(Request.Cookies);
                int uid = user.Id;
                UserModel nwUser = new UserModel();
                var udata = GetUser(uid);
                bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(userModel.ConfirmPassword, udata.Password);
                if (isCorrectPassword)
                {
                    nwUser.Id = userModel.Id;
                    nwUser.Name = userModel.Name;
                    nwUser.DateOfBirth = userModel.DateOfBirth;
                    nwUser.Address = userModel.Address;
                    nwUser.CityId = userModel.CityId;
                    nwUser.Username = userModel.Username;
                    nwUser.Password = userModel.Password;
                    nwUser.Email = userModel.Email;
                    nwUser.Phone = userModel.Phone;
                    nwUser.Role = 2;
                    db.Entry(udata).CurrentValues.SetValues(nwUser);
                    db.SaveChanges();
                }
                TempData["msg"] = "Incorrect Password";
            }
            return View(userModel);
        }

        [HttpGet]
        public string BookFlight(int transportId, string dateTime)
        {
            DateTime dt = DateTime.Parse(dateTime);
            return dt.ToString() + transportId.ToString();
        }

        

        public User GetUser(int uid)
        {
            var data = (from u in db.Users
                        where u.Id == uid
                        select u).FirstOrDefault();
            return data;
        }
    }
}
