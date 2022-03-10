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
            var users = db.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            //int aid = (int)(Session["uid"]);
            int uid = 9;
            UserModel userModel = new UserModel();
            var udata = GetUser(uid);
            userModel.Id = udata.Id;
            userModel.Name = udata.Name;
            userModel.Cell = udata.Phones.Select(p => p.Phone1).FirstOrDefault();
            userModel.Mail = udata.Emails.Select(e => e.Email1).FirstOrDefault();
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

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
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

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
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

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
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

        public User GetUser(int uid)
        {
            var data = (from u in db.Users
                        where u.Id == uid
                        select u).FirstOrDefault();
            return data;
        }

        public Email GetEmail(int uid)
        {
            var data = (from m in db.Emails
                        where m.UserId == uid
                        select m).FirstOrDefault();
            return data;
        }

        public Phone GetPhone(int uid)
        {
            var data = (from m in db.Phones
                        where m.UserId == uid
                        select m).FirstOrDefault();
            return data;
        }
    }
}
