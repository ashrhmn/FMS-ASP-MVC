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
            user.Name = data.Name;
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
            user.Id = data.Id;
            user.Name = data.Name;
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
                user.Password= data.Password;
                user.CityId = data.CityId;
                user.FamilyId = data.FamilyId;
                user.Role = data.Role;

                db.Entry(data).CurrentValues.SetValues(user);
                db.SaveChanges();



                var emails = (from e in db.Emails where e.UserId == data.Id select e).ToList();
                var phone = (from p in db.Phones where p.UserId == data.Id select p).ToList();


                TempData["msg"] = "Profile Updated Successfully";
                return RedirectToAction("Details");
            }
            TempData["msg"] = "Information is not correct";
            return View();
            
        }
        [HttpGet]
        public ActionResult ChangePass()
        {
            //Flight_ManagementEntities db = new Flight_ManagementEntities();
            //var data = (from a in db.Users where a.Username.Equals("Ashik") select a.Password).FirstOrDefault();
            //var user = new UserModel();
            //user.Password = data;
            return View(new UserModel());

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
                        Id = data.Id,
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
        public ActionResult PurchasedUserList()
        {
            Flight_ManagementEntities db = new Flight_ManagementEntities();
            var data = db.Users.ToList();
            var user = new List<UserModel>();
            foreach(var u in data)
            {
                user.Add(new UserModel()
                {
                    Id =u.Id,
                    Name = u.Name,
                    Username = u.Username,
                    PurchasedTickets = u.PurchasedTickets.Select(e => e.Id).ToList(),

            });
            }

            return View(user);
        }
        public ActionResult PurchasedDetails(int id)
        {
            Flight_ManagementEntities db = new Flight_ManagementEntities();
            var purchasedetails = (from pd in db.PurchasedTickets where pd.PurchasedBy == id select pd).ToList();
            var user = (from u in db.Users where u.Id == id select u).FirstOrDefault();
        
            var pdetails = new List<PurchasedTicketModel>();

            foreach (var pd in purchasedetails)
            {
                var fromstopage = (from fs in db.Cities where fs.Id == pd.FromStopageId select fs).FirstOrDefault();
                var tostopage = (from ts in db.Cities where ts.Id == pd.ToStopageId select ts).FirstOrDefault();
                var seatInfo = (from si in db.SeatInfos where si.TicketId==pd.Id select si).FirstOrDefault();
                var seatclass = (from sc in db.SeatClassEnums where sc.Id == seatInfo.SeatClass select sc).FirstOrDefault();
                var ageclass = (from ac in db.AgeClassEnums where ac.Id == seatInfo.AgeClass select ac).FirstOrDefault();
                var trans = (from t in db.Transports where t.Id == seatInfo.TransportId select t).FirstOrDefault();
                
                pdetails.Add(new PurchasedTicketModel()
                {
                    Id = pd.Id,
                    PurchasedByName =user.Name,
                    FromStopageCityName = fromstopage.Name,
                    FromStopageCountryName = fromstopage.Country,
                    ToStopageCityName= tostopage.Name,
                    ToStopageCountryName= tostopage.Country,
                    StartTime = seatInfo.StartTime,
                    SeatNo = seatInfo.SeatNo,
                    SeatClass = seatclass.Value,
                    AgeClass = ageclass.Value,
                    TransportCreatedBy= trans.CreatedBy,
                    TransportName= trans.Name,
                    TransportId = trans.Id,
                });
            }
            return View(pdetails);
        }


        
    }
}