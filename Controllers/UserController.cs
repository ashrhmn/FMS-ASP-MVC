using Flight_Management_System.Auth;
using Flight_Management_System.Models;
using Flight_Management_System.Models.AuthEntities;
using Flight_Management_System.Models.Database;
using Flight_Management_System.Utils;
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
            string Day = flMod.Date.ToString("dddd");
            var transports = (from fs in db.TransportSchedules where fs.FromStopageId == flMod.FromStopageId 
                               && fs.ToStopageId == flMod.ToStopageId && fs.Day.Equals(Day) select fs).ToList();
            var flights = new List<TransportModelSR>();
            foreach (var f in transports) 
            {

                var occupiedSeats = (from s in db.SeatInfos where s.TransportId == f.TransportId && s.Status == "Booked" select s).Count();
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
                StartTime = flMod.Date,
                TicketId = tkt.Id,
                SeatClass = flMod.ClassId,
                AgeClass = 1,
                Status = "Booked",
                TransportId = flMod.TransportId,
            };
            db.SeatInfos.Add(seat);
            db.SaveChanges();

            return View(flMod);
        }

        [HttpGet]
        public ActionResult Tickets()
        {
            AuthPayload user = jwt.LoggedInUser(Request.Cookies);
            int uid = user.Id;
            var custTik = new List<CustomerFlightSR>();
            var udata = GetUser(uid);
            var tkts = (from t in db.PurchasedTickets where t.PurchasedBy == udata.Id select t).ToList();
            foreach(var t in tkts)
            {
                var seatIn = (from s in db.SeatInfos where s.TicketId == t.Id select s).FirstOrDefault();
                var sno = seatIn.SeatNo;
                var scId = seatIn.SeatClass;
                var from = (from f in db.Stopages where f.Id == t.FromStopageId select f.Name).FirstOrDefault();
                var to = (from ft in db.Stopages where ft.Id == t.ToStopageId select ft.Name).FirstOrDefault();
                var sCls = (from ft in db.SeatClassEnums where ft.Id == scId select ft.Value).FirstOrDefault();
                var sTime = seatIn.StartTime;
                var tName = (from n in db.Transports where n.Id == seatIn.TransportId select n.Name).FirstOrDefault();

             custTik.Add(new CustomerFlightSR()
            {
                SeatNo = sno,
                StartTime = sTime,
                SeatClass = sCls,
                ToStopage = to,
                FromStopage = from,
                Status = seatIn.Status,
                TName = tName,
            });


            }
            return View(custTik);
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
                UserModelSR nwUser = new UserModelSR();
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
