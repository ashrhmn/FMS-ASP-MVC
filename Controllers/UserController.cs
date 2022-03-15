using Flight_Management_System.Auth;
using Flight_Management_System.Models;
using Flight_Management_System.Models.AuthEntities;
using Flight_Management_System.Models.Database;
using Flight_Management_System.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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

            return View(new List<TransportModelSR>());
        }
        
        [HttpPost]
        public ActionResult Flights(TransportModelSR flMod)
        {
            DateTime CurrentDate = DateTime.Now.Date;
            DateTime SearchedDate = flMod.Date.Date;
            int previousDate = DateTime.Compare(SearchedDate, CurrentDate);
            if (previousDate < 0)
            {
                TempData["msg"] = "You Can not search Flight for previous days";
                return View(new List<TransportModelSR>());
            }

            string Day = flMod.Date.ToString("dddd");
            var fsi = (from fs in db.Stopages
                              where fs.Name == flMod.SFS
                              select fs.Id).FirstOrDefault();

            var tsi = (from fs in db.Stopages
                       where fs.Name == flMod.STS
                       select fs.Id).FirstOrDefault();

            var transports = (from fs in db.TransportSchedules
                              where fs.FromStopageId == fsi
                               && fs.ToStopageId == tsi && fs.Day.Equals(Day)
                              select fs).ToList();
            if (!transports.Any())
            {
                TempData["msg"] = "Sorry No Available Flights Found for your schedule";
                return View(new List<TransportModelSR>());
            }
            var flights = new List<TransportModelSR>();
            foreach (var f in transports) 
            {

                var occupiedSeats = (from s in db.SeatInfos where s.TransportId == f.TransportId && s.Status == "Booked"  select s).Count();
                var maxSeat = (from s in db.Transports where s.Id == f.TransportId select s.MaximumSeat).FirstOrDefault();
                var availableSeats = maxSeat - occupiedSeats;
                var FromRootFare = (from t in db.Stopages where t.Id == f.FromStopageId select t.FareFromRoot).FirstOrDefault();
                var ToRootFare = (from t in db.Stopages where t.Id == f.ToStopageId select t.FareFromRoot).FirstOrDefault();
                var baseFare = Math.Abs((FromRootFare ?? -1) - (ToRootFare ?? -1));
                var Fare = 0;
                var sClass = "class";
                Session["classId"] = flMod.ClassId;

                if (flMod.ClassId == 1) 
                {
                    sClass = "Business";
                    Fare = baseFare * 15;
                }
                else if (flMod.ClassId == 2)
                {
                    Fare = baseFare * 10;
                    sClass = "Economy";
                }
                else 
                {
                    sClass = "Business Economy";
                    Fare = baseFare * 12;
                }
                string time = ((int)f.Time / 100) + ":" + ((f.Time % 100).ToString() + ":" + 00);
                var result = Convert.ToDateTime(time);
                var dateOnly = flMod.Date.Date;
                DateTime startDate = dateOnly.Add(result.TimeOfDay);
                Session["jTime"] = startDate;
              
                flights.Add(new TransportModelSR()
                {
                    Id = f.Id,
                    TransportId = f.TransportId,
                    Name = f.Transport.Name,
                    FromStopage = (from s in db.Stopages where s.Id == f.FromStopageId select s.Name).FirstOrDefault(),
                    ToStopage = (from t in db.Stopages where t.Id == f.ToStopageId select t.Name).FirstOrDefault(),
                    Day = f.Day,
                    AvailableSeats = availableSeats,
                    Date = startDate,
                    SFare = Fare,

                });
            }
            return View(flights);
        }

        [HttpGet]
        public ActionResult Book(int Id)
        {
            AuthPayload user = jwt.LoggedInUser(Request.Cookies);
            int uid = user.Id;
            var transport = (from fs in db.TransportSchedules
                             where fs.Id == Id
                             select fs).FirstOrDefault();
            var udata = GetUser(uid);
            var uname = udata.Name;
            var uphone = udata.Phone;

            var occupiedSeats = (from s in db.SeatInfos where s.TransportId == transport.TransportId && s.Status == "Booked" select s).Count();
            var maxSeat = (from s in db.Transports where s.Id == transport.TransportId select s.MaximumSeat).FirstOrDefault();
            var availableSeats = maxSeat - occupiedSeats;
            var FromRootFare = (from t in db.Stopages where t.Id == transport.FromStopageId select t.FareFromRoot).FirstOrDefault();
            var ToRootFare = (from t in db.Stopages where t.Id == transport.ToStopageId select t.FareFromRoot).FirstOrDefault();
            var baseFare = Math.Abs((FromRootFare ?? -1) - (ToRootFare ?? -1));
            var Fare = 0;
            var sclId = (int)Session["classId"];
            var sClass = "Test";
            if (sclId == 1)
            {
                sClass = "Business";
                Fare = baseFare * 15;
                
            }
            else if (sclId == 2)
            {
                Fare = baseFare * 10;
                sClass = "Economy";
            }
            else
            {
                sClass = "Economic Business";
                Fare = baseFare * 12;
            }
            var vFlit = new TransportModelSR()
            {
                Id = transport.Id,
                Name = (from t in db.Transports where t.Id == transport.TransportId select t.Name).FirstOrDefault(),
                TransportId = transport.TransportId,
                FromStopageId = transport.FromStopageId ?? -1,
                ToStopageId = transport.ToStopageId ?? -1,
                FromStopage = (from t in db.Stopages where t.Id == transport.FromStopageId select t.Name).FirstOrDefault(),
                ToStopage = (from t in db.Stopages where t.Id == transport.ToStopageId select t.Name).FirstOrDefault(),
                SFare = Fare,
                ClassId = (int)Session["classId"],
                Class = sClass,
                AvailableSeats = availableSeats,
                Date = (DateTime)Session["jTime"],
                Cname = uname,
                Cphone = uphone,
            };

            return View(vFlit);
        }

        [HttpPost]
        public ActionResult Book(TransportModelSR flMod)
        {
            AuthPayload user = jwt.LoggedInUser(Request.Cookies);
            int uid = user.Id;

            var tickt = new PurchasedTicket()
            {
                FromStopageId = flMod.FromStopageId,
                ToStopageId = flMod.ToStopageId,
                PurchasedBy = uid,
            };
            var tkt = db.PurchasedTickets.Add(tickt);
            db.SaveChanges();

            var seat = new SeatInfo()
            {
                SeatNo = flMod.AvailableSeats - 1,
                StartTime = flMod.MDate,
                TicketId = tkt.Id,
                SeatClass = flMod.ClassId,
                AgeClass = 1,
                Status = "Booked",
                TransportId = flMod.TransportId,
            };
            db.SeatInfos.Add(seat);
            db.SaveChanges();

            Session["classId"] = null;
            Session["jTime"] = null;
            return RedirectToAction("Tickets", "User");

        }

        [HttpGet]
        public ActionResult Tickets()
        {
            AuthPayload user = jwt.LoggedInUser(Request.Cookies);
            int uid = user.Id;
            var custTik = new List<CustomerFlightSR>();
            var temp = db.SeatInfos.Where(s => s.PurchasedTicket.User.Id == user.Id && s.Status !="Cancelled").ToList();
            if (temp != null)
            {
                foreach (var t in temp)
                {
                    var sno = t.SeatNo;
                    var scId = t.SeatClass;
                    var from = t.PurchasedTicket.Stopage.City.Name + " , " + t.PurchasedTicket.Stopage.Name;
                    var To = t.PurchasedTicket.Stopage1.City.Name + " , " + t.PurchasedTicket.Stopage1.Name;
                    var scName = t.SeatClassEnum.Value;
                    var FromRootFare = t.PurchasedTicket.Stopage.FareFromRoot;
                    var ToRootFare = t.PurchasedTicket.Stopage1.FareFromRoot; ;
                    var baseFare = Math.Abs((FromRootFare ?? -1) - (ToRootFare ?? -1));
                    var sTime = t.StartTime;
                    var tName = t.Transport.Name;
                    var Fare = 0;
                    if (scId == 1)
                    {
                        Fare = baseFare * 15;

                    }
                    else if (scId == 2)
                    {
                        Fare = baseFare * 10;
                    }
                    else
                    {
                        Fare = baseFare * 12;
                    }

                    custTik.Add(new CustomerFlightSR()
                    {
                        TktId = t.TicketId ?? -1,
                        SeatNo = sno,
                        StartTime = sTime,
                        SeatClass = scName,
                        ToStopage = To,
                        FromStopage = from,
                        Status = t.Status,
                        TName = tName,
                        TFare = Fare,
                    });


                }
                return View(custTik);
            }
            return View(new List<CustomerFlightSR>());
        }

        [HttpGet]
        public ActionResult CanceledTickets()
        {
            AuthPayload user = jwt.LoggedInUser(Request.Cookies);
            int uid = user.Id;
            var custTik = new List<CustomerFlightSR>();
            var temp = db.SeatInfos.Where(s => s.PurchasedTicket.User.Id == user.Id && s.Status.Equals("Cancelled")).ToList();
            if (temp != null)
            {
                foreach (var t in temp)
                {
                    var sno = t.SeatNo;
                    var scId = t.SeatClass;
                    var from = t.PurchasedTicket.Stopage.City.Name + " , " + t.PurchasedTicket.Stopage.Name;
                    var To = t.PurchasedTicket.Stopage1.City.Name + " , " + t.PurchasedTicket.Stopage1.Name;
                    var scName = t.SeatClassEnum.Value;
                    var FromRootFare = t.PurchasedTicket.Stopage.FareFromRoot;
                    var ToRootFare = t.PurchasedTicket.Stopage1.FareFromRoot; ;
                    var baseFare = Math.Abs((FromRootFare ?? -1) - (ToRootFare ?? -1));
                    var sTime = t.StartTime;
                    var tName = t.Transport.Name;
                    var Fare = 0;
                    if (scId == 1)
                    {
                        Fare = baseFare * 15;

                    }
                    else if (scId == 2)
                    {
                        Fare = baseFare * 10;
                    }
                    else
                    {
                        Fare = baseFare * 12;
                    }

                    custTik.Add(new CustomerFlightSR()
                    {
                        TktId = t.TicketId ?? -1,
                        SeatNo = sno,
                        StartTime = sTime,
                        SeatClass = scName,
                        ToStopage = To,
                        FromStopage = from,
                        Status = t.Status,
                        TName = tName,
                        TFare = Fare,
                    });


                }
                return View(custTik);
            }
            return View(new List<CustomerFlightSR>());
        }

        [HttpGet]
        public ActionResult Cancel(int id)
        {
            AuthPayload user = jwt.LoggedInUser(Request.Cookies);
            int uid = user.Id;
            var udata = GetUser(uid);
            var t = (from tkt in db.PurchasedTickets where tkt.Id == id && tkt.PurchasedBy == udata.Id select tkt).FirstOrDefault();
            var seatIn = (from s in db.SeatInfos where s.TicketId == t.Id select s).FirstOrDefault();
            var newSeatIn = new SeatInfo()
            {
                Id = seatIn.Id,
                StartTime = seatIn.StartTime,
                SeatNo = seatIn.SeatNo,
                TicketId = seatIn.TicketId,
                TransportId = seatIn.TransportId,
                AgeClass = seatIn.AgeClass,
                SeatClass = seatIn.SeatClass,
                Status = "Pending Cancelation",
            };
            db.Entry(seatIn).CurrentValues.SetValues(newSeatIn);
            db.SaveChanges();
            
            return RedirectToAction("Tickets", "User");
                

         }


        [HttpGet]
        public ActionResult Dashboard()
        {
            AuthPayload user = jwt.LoggedInUser(Request.Cookies);
            int uid = user.Id;
            UserModelSR userModel = new UserModelSR();
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
        public ActionResult Dashboard(UserModelSR userModel)
        {
            if (ModelState.IsValid)
            {
                AuthPayload user = jwt.LoggedInUser(Request.Cookies);
                int uid = user.Id;
                var udata = GetUser(uid);
                bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(userModel.ConfirmPassword, udata.Password);
                if (isCorrectPassword)
                {
                    UserModelSR nwUser = new UserModelSR();
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
                else { TempData["msg"] = userModel.ConfirmPassword; }
                //TempData["msg"] = userModel.ConfirmPassword;
            }
            return View(userModel);
        }

        [HttpGet]
        public string BookFlight(int transportId, string dateTime)
        {
            DateTime dt = DateTime.Parse(dateTime);
            return dt.ToString() + transportId.ToString();
        }

        public JsonResult GetAirports(string search)
        {
            List <AirportModelSR> allsearch = db.Stopages.Where(x => x.Name.Contains(search)).Select(x => new AirportModelSR
            {
                Aid = x.Id,
                Aname = x.Name,
                Acity = x.City.Name
            }).ToList();
            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
