using Flight_Management_System.Models;
using Flight_Management_System.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flight_Management_System.Controllers
{
    public class UserController : Controller
    {
        private Flight_ManagementEntities db;
        public UserController()
        {
            db = new Flight_ManagementEntities();
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

        [HttpPost]
        public ActionResult Flights(Transport flMod)
        {
            var data = (from t in db.Transports
                        where t.FromStopageId == flMod.FromStopageId && t.ToStopageId == flMod.ToStopageId
                        select t).ToList();
            return View(data);
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            //int aid = (int)(Session["uid"]);
            int uid = 13;
            UserModel userModel = new UserModel();
            var udata = GetUser(uid);
            userModel.Id = udata.Id;
            userModel.Name = udata.Name;
            userModel.Mail = udata.;
            userModel.Address = udata.Address;
            userModel.CityId = udata.CityId;
            userModel.CityName = udata.City.Name;
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
                int uid = 9;
                UserModel nwUser = new UserModel();
                var udata = GetUser(uid);
                nwUser.Id = userModel.Id;
                nwUser.Name = userModel.Name;
                nwUser.DateOfBirth = userModel.DateOfBirth;
                nwUser.Address = userModel.Address;
                nwUser.CityId = userModel.CityId;
                nwUser.Username = userModel.Username;
                nwUser.Password = userModel.Password;
                nwUser.Role = 2;
                db.Entry(udata).CurrentValues.SetValues(nwUser);
                db.SaveChanges();
                var edata = GetEmail(uid);
                Email neEmail = new Email();
                neEmail.Id = edata.Id;
                neEmail.UserId = edata.UserId;
                neEmail.Email1 = userModel.Mail;
                db.Entry(edata).CurrentValues.SetValues(neEmail);
                db.SaveChanges();
                var pdata = GetPhone(uid);
                Phone nePhone = new Phone();
                nePhone.Id = pdata.Id;
                nePhone.UserId = pdata.UserId;
                nePhone.Phone1 = userModel.Cell;
                db.Entry(pdata).CurrentValues.SetValues(nePhone);
                db.SaveChanges();
            }
            return View(userModel);
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
