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
            //int aid = (int)(Session["aid"]);
            int aid = 1;
            UserModel userModel = new UserModel();
            var udata = GetUser(aid);
            userModel.Id = udata.Id;
            userModel.Name = udata.Name;
            userModel.Address = udata.Address;
            userModel.DateOfBirth = udata.DateOfBirth;
            userModel.Password = udata.Password;
            return View(userModel);
        }

        [HttpPost]
        public ActionResult Dashboard(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                /*var airline = (from a in db.Airlines where a.airline_id == alrvm.airline_id select a).FirstOrDefault();
                Airline newAl = new Airline();
                newAl.airline_id = alrvm.airline_id;
                newAl.airline_name = alrvm.airline_name;
                newAl.airline_regno = alrvm.airline_regno;
                newAl.airline_address = alrvm.airline_address;
                newAl.airline_phone = alrvm.airline_phone;
                newAl.airline_status = "Valid";
                db.Entry(airline).CurrentValues.SetValues(newAl);
                db.SaveChanges();
                var login = (from als in db.Logins where als.airline_id == alrvm.airline_id select als).FirstOrDefault();
                Login lg = new Login();
                lg.login_id = login.login_id;
                lg.airline_id = alrvm.airline_id;
                lg.username = alrvm.username;
                lg.password = alrvm.password;
                lg.recovery_phone = alrvm.recovery_phone;
                lg.email = alrvm.email;
                lg.user_type = "airline";
                db.Entry(login).CurrentValues.SetValues(lg);
                db.SaveChanges();

                return RedirectToAction("Dashboard");*/
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

        public User GetUser(int aid)
        {
            var data = (from u in db.Users
                        where u.Id == aid
                        select u).FirstOrDefault();
            return data;
        }
    }
}
