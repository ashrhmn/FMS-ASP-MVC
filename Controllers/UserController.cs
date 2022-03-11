using Flight_Management_System.Auth;
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
            var udata = (from u in db.Users
                         where u.Id == 14
                         select u).FirstOrDefault();
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
                //db.SaveChanges();
                //var edata = GetEmail(uid);
                //Email neEmail = new Email();
                //neEmail.Id = edata.Id;
                //neEmail.UserId = edata.UserId;
                //neEmail.Email1 = userModel.Mail;
                //db.Entry(edata).CurrentValues.SetValues(neEmail);
                //db.SaveChanges();
                //var pdata = GetPhone(uid);
                //Phone nePhone = new Phone();
                //nePhone.Id = pdata.Id;
                //nePhone.UserId = pdata.UserId;
                //nePhone.Phone1 = userModel.Cell;
                //db.Entry(pdata).CurrentValues.SetValues(nePhone);
                db.SaveChanges();
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
