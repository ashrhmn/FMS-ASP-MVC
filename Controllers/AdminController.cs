using Flight_Management_System.Models.AdminEntities;
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
    public class AdminController : Controller
    {
        private Flight_ManagementEntities db;
        private JwtManage jwt;
        public AdminController()
        {
            db = new Flight_ManagementEntities();
            jwt = new JwtManage();
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details()
        {
            AuthPayload payload = jwt.LoggedInUser(Request.Cookies);
            var data = (from a in db.Users where a.Username.Equals(payload.Username) select a).FirstOrDefault();
            if(data == null)
            {
                jwt.DeleteToken(Request.Cookies);
                return RedirectToAction("Signin", "Auth");
            }
            if (data.UserRoleEnum.Value != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            var user = new UserModel();
            user.Name = data.Name;
            user.Username = data.Username;
            user.DateOfBirth = data.DateOfBirth;
            //user.CityName = data.City==null?"Undefined":data.City.Name;
            //user.CountryName = data.City == null ? "Undefined" : data.City.Country;
            user.Address= data.Address;
            user.Email = data.Email;
            user.Phone = data.Phone;

            return View(user);
        }
        [HttpGet]
        public ActionResult EditProfile()
        {
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
            user.Email = data.Email;
            user.Phone = data.Phone;

            return View(user);
        }
        [HttpPost]
        public ActionResult EditProfile(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var data = (from a in db.Users where a.Username.Equals("Ashik") select a).FirstOrDefault();
                user.Password= data.Password;
                user.CityId = data.CityId;
                user.FamilyId = data.FamilyId;
                user.Role = data.Role;
                user.Email = data.Email;
                user.Phone = data.Phone;

                db.Entry(data).CurrentValues.SetValues(user);
                db.SaveChanges();


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
                var data = (from a in db.Users where a.Username.Equals("Ashik") select a).FirstOrDefault();

                bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(OldPassword, data.Password);

                if (isCorrectPassword)
                {
                    var user = new UserModel()
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Username = data.Username,
                        Password = BCrypt.Net.BCrypt.HashPassword(Password, 12),  // this Bcrypt
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
        public ActionResult Userlist(string searching)
        {
            Flight_ManagementEntities db = new Flight_ManagementEntities();
            var user = (from u in db.Users select u).ToList();
            if (!String.IsNullOrEmpty(searching))
            {
                user = user.Where(u => u.Name.Contains(searching)).ToList();
            }
            var users = new List<UserModel>();
            foreach (var u in user)
            {
                users.Add(new UserModel()
                {
                    Id = u.Id,
                    Name = u.Name,
                    Username = u.Username,
                    DateOfBirth = u.DateOfBirth,
                    Address = u.Address,
                    Role = u.Role,
                    CityName = u.City == null ? "undefined" : u.City.Name,
                    CountryName = u.City == null ? "undefined" : u.City.Country,
                    Email = u.Email == null ? "undefined" : u.Email,
                    Phone = u.Phone == null ? "undefined" : u.Phone,

                });
            }
            return View(users);
        }
        [HttpGet]
        public ActionResult EditUserProfile(int id)
        {
            var u = (from us in db.Users where us.Id==id select us).FirstOrDefault();

            var user = new UserModel()
            {
                Id = u.Id,
                Name = u.Username,
                Username = u.Username,
                DateOfBirth = u.DateOfBirth,
                Address = u.Address,
                Role = u.Role,
                //CityName = u.City == null ? "undefined" : u.City.Name,
                //CountryName = u.City == null ? "undefined" : u.City.Country,
                Email = u.Email,
                Phone = u.Phone,


            };
            
            return View(user);
        }
        public ActionResult DeleteUser(int id)
        {
            var user = (from u in db.Users where u.Id == id select u).FirstOrDefault();
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Userlist", "Admin");
        }
        [HttpGet]
        public ActionResult ChangeUserPass(int id)
        {
            ViewBag.id = id;
            return View();

        }
        [HttpPost]
        public ActionResult ChangeUserPass(int id,string OldPassword, string Password, string ConPassword)
        {
            if (Password.Equals(ConPassword))
            {
                var data = (from a in db.Users where a.Id == id select a).FirstOrDefault();

                bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(OldPassword, data.Password);

                if (isCorrectPassword)
                {
                    var user = new UserModel()
                    {
                        Name = data.Name,
                        Username = data.Username,
                        Password = BCrypt.Net.BCrypt.HashPassword(Password, 12),  // this Bcrypt
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
        public ActionResult Flights(string searching)
        {
            Flight_ManagementEntities db = new Flight_ManagementEntities();
            var flight = db.Transports.ToList();
            if (!String.IsNullOrEmpty(searching))
            {
                flight = flight.Where(u => u.Name.Contains(searching)).ToList();
            }
            var flights = new List<TransportModel>();
            foreach (var f in flight)
            {
                var occupiedSeats = (from s in db.SeatInfos where s.TransportId == f.Id && s.Status == "Booked" select s).Count();
                var availableSeats = (f.MaximumSeat - occupiedSeats);


                flights.Add(new TransportModel()
                {
                    Id = f.Id,
                    Name = f.Name,
                    //From = f.TransportSchedules.Stopage,
                    //Destination = f.ToStopageId == null ? "undefined" : f.TransportSchedules.s,
                    MaximumSeat = f.MaximumSeat,
                    CreatorName = f.CreatedBy == null ? "undefined" : f.User.Name,
                    AvailableSeats = availableSeats,

                });
            }
            return View(flights);
        }
        public ActionResult BookedSeatsFlights(int id)
        {
            Flight_ManagementEntities db = new Flight_ManagementEntities();
            var seat = (from s in db.SeatInfos where s.Id == id && s.Status == "Booked" select s).ToList();
            var seats = new List<SeatInfosModel>();
            foreach (var s in seat)
            {
                seats.Add(new SeatInfosModel()
                {
                    Id = s.Id,
                    SeatNo = s.SeatNo,
                    SeatClassName = s.SeatClass == null ? "undefined" : s.SeatClassEnum.Value,
                    PurchasedById = s.PurchasedTicket.PurchasedBy,
                    PurchasedByName = s.TicketId == null ? "undefined" : s.PurchasedTicket.User.Name,
                    Status = s.Status,

                });
            }

            return View(seats);
        }
        public ActionResult UserDetails(int id)
        {
            Flight_ManagementEntities db = new Flight_ManagementEntities();
            var data = (from a in db.Users where a.Id == id select a).FirstOrDefault();
            var user = new UserModel();
            user.Username = data.Username;
            user.DateOfBirth = data.DateOfBirth;
            //user.CityName = data.City==null?"Undefined":data.City.Name;
            //user.CountryName = data.City == null ? "Undefined" : data.City.Country;
            user.Address = data.Address;
            user.Email = data.Email;
            user.Phone = data.Phone;

            return View(user);
        }


        public ActionResult TransportDetails(int id)
        {
            var transport = (from t in db.Transports where t.Id == id select t).FirstOrDefault();
            if(transport == null)
            {
                var user = (from u in db.Users where u.Id == transport.CreatedBy select u).FirstOrDefault();
                //var fromstopage = (from fs in db.Cities where fs.Id == transport.FromStopageId select fs).FirstOrDefault();
                //var tostopage = (from ts in db.Cities where ts.Id == transport.ToStopageId select ts).FirstOrDefault();

                var trans = new TransportModel();
                trans.Id = id;
                trans.Name = transport.Name;
                //trans.From = fromstopage.Name;
                //trans.FromCountry = fromstopage.Country;
                //trans.Destination = tostopage.Name;
                //trans.DestinationCountry = tostopage.Country;
                trans.CreatedBy = user.Id;
                trans.CreatorName = user.Name;
                trans.MaximumSeat = transport.MaximumSeat;


                return View(trans);
            }
            return null;
            
        }
        public ActionResult CancelTicket(int id)
        {
            var ticket = (from t in db.PurchasedTickets where t.Id == id select t).FirstOrDefault();
            var seat = (from s in db.SeatInfos where s.TicketId == id select s).FirstOrDefault();
            
            db.SeatInfos.Remove(seat);
            db.SaveChanges();
            db.PurchasedTickets.Remove(ticket);
            db.SaveChanges();

            TempData["msg"] = "Ticket Cancel Successfully";
            return RedirectToAction("PurchasedDetails");
        }
        [HttpGet]
        public ActionResult SearchFlight()
        {
            var data = (from cs in db.Cities select cs).ToList();
            var cities = new List<CityModel>();

            foreach(var c in data)
            {
                cities.Add(new CityModel()
                {
                    Name = c.Name,
                });
            }

            return View(cities);
        }
        [HttpPost]
        public ActionResult SearchFlight(string Date, string From, string Destination)
        {
            
            var cities = (from cs in db.Cities select cs);
            if (From != null && Destination != null)
            {
                var fromcity = cities.Where(cs => cs.Name.Contains(From)).FirstOrDefault();
                var destinationcity = cities.Where(cs => cs.Name.Contains(Destination)).FirstOrDefault();
                var flights = (from fs in db.TransportSchedules where fs.FromStopageId == fromcity.Id 
                               && fs.ToStopageId == destinationcity.Id && fs.Day.Equals(Date) select fs).FirstOrDefault();

                if(flights != null)
                {
                    var transports = (from t in db.Transports where t.Id == flights.TransportId select t).FirstOrDefault();
                    var trans = new TransportModel()
                    {
                        Name = transports.Name,
                        Id = flights.TransportId,
                    };
                    return View(trans);

                }
                

            }

            return View();
        }
        public ActionResult FlightSeatDetails()
        {
            return View();
        }




    }
}