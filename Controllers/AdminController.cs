using Flight_Management_System.Auth;
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

    [AdminAccess]

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



        /////////////// Evan Start/////////////////////// 
    
       public ActionResult SearchAllUsers()
        {
            return View();
        }


        public ActionResult Details()
        {
            AuthPayload loggeduser = jwt.LoggedInUser(Request.Cookies); 
            var data = (from a in db.Users where a.Username.Equals(loggeduser.Username) select a).FirstOrDefault();
            
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
            AuthPayload loggeduser = jwt.LoggedInUser(Request.Cookies);
            var data = (from a in db.Users where a.Username.Equals(loggeduser.Username) select a).FirstOrDefault();

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
                AuthPayload loggeduser = jwt.LoggedInUser(Request.Cookies);
                var data = (from a in db.Users where a.Username.Equals(loggeduser.Username) select a).FirstOrDefault();
                user.Password= data.Password;
                user.CityId = data.CityId;
                user.FamilyId = data.FamilyId;
                user.Role = data.Role;
                //user.Email = data.Email;
                //user.Phone = data.Phone;

                db.Entry(data).CurrentValues.SetValues(user);
                db.SaveChanges();


                TempData["msg"] = "Profile Updated Successfully";
                return RedirectToAction("Details");
            }
            //TempData["msg"] = "Information is not correct";
            return View(user);
            
        }
        [HttpGet]
        public ActionResult ChangePass()
        {
            //AuthPayload loggeduser = jwt.LoggedInUser(Request.Cookies);
            //var data = (from a in db.Users where a.Username.Equals("Ashik") select a.Password).FirstOrDefault();
            //var user = new UserModel();
            //user.Password = data;
            return View(new UserModel());

        }
        [HttpPost]
        public ActionResult ChangePass(string OldPassword, string Password, string ConPassword)
        {
            AuthPayload loggeduser = jwt.LoggedInUser(Request.Cookies);
            if (Password.Equals(ConPassword))
            {
                var data = (from a in db.Users where a.Username.Equals(loggeduser.Username) select a).FirstOrDefault();

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
                        Email = data.Email,
                        Phone = data.Phone
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

        [HttpGet]

        public ActionResult PurchasedUserList()
        {
            var data = (from u in db.Users where u.Role == 2 select u).ToList();
            var user = new List<UserModel>();
            foreach (var u in data)
            {
                user.Add(new UserModel()
                {
                    Id = u.Id,
                    Name = u.Name,
                    Username = u.Username,
                    PurchasedTickets = u.PurchasedTickets.Select(e => e.Id).ToList(),

                });
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult PurchasedUserList(string Uname, string Purchase)
        {
            List<User> data;
            if(Uname == "" && Purchase != "true")
            {
                data = (from u in db.Users where u.Role == 2 select u).ToList();
            }
            else if(Uname != "" && Purchase != "true")
            {
                data = (from u in db.Users where u.Role == 2 && u.Username.Contains(Uname) select u).ToList();
            }
            else if (Uname == "" && Purchase == "true")
            {
                data = db.PurchasedTickets.Select(pb => pb.User).ToList();
            }
            else if (Uname != "" && Purchase == "true")
            {
                data = db.PurchasedTickets.Select(pb => pb.User).Where(pw=>pw.Username.Contains(Uname)).ToList();
            }
            else
            {
                data = null;
            }
            if (data!=null)
            {
                var users = new List<UserModel>();
                foreach (var disUser in data)
                {
                    if (!users.Select(u => u.Id).ToList().Contains(disUser.Id))
                    {
                        users.Add(new UserModel()
                        {
                            Name = disUser.Name,
                            Username = disUser.Username,
                            Id = disUser.Id,
                            PurchasedTickets = disUser.PurchasedTickets.Select(e => e.Id).ToList(),
                        });
                    }
                }
                return View(users);
            }
            return null;


        }
        [HttpGet]
        public ActionResult FlightManagerList()
        {
            var data = (from u in db.Users where u.Role == 3 select u).ToList();
            
            var user = new List<UserModel>();
            foreach (var u in data)
            {
                var trans = (from t in db.Transports where t.CreatedBy == u.Id select t).FirstOrDefault();
                user.Add(new UserModel()
                {
                    Id = u.Id,
                    Name = u.Name,
                    Username = u.Username,
                    PurchasedTickets = trans==null?new List<int>(): trans.TransportSchedules.Select(e => e.Id).ToList(),

                });
            }

            return View(user);

        }

        [HttpPost]
        public ActionResult FlightManagerList(string Uname)
        {
            //List<User> data;
            if (Uname == "" )
            {
                var data = (from u in db.Users where u.Role == 3 select u).ToList();

                var user = new List<UserModel>();
                foreach (var u in data)
                {
                    var trans = (from t in db.Transports where t.CreatedBy == u.Id select t).FirstOrDefault();
                    user.Add(new UserModel()
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Username = u.Username,
                        PurchasedTickets = trans == null ? new List<int>() : trans.TransportSchedules.Select(e => e.Id).ToList(),

                    });
                }

                return View(user);
            }
            else if (Uname != "")
            {
                //data = (from u in db.Users where u.Role == 2 && u.Username.Contains(Uname) select u).ToList();
                var data = (from u in db.Users where u.Role == 3 && u.Username.Contains(Uname) select u).ToList();

                var user = new List<UserModel>();
                foreach (var u in data)
                {
                    var trans = (from t in db.Transports where t.CreatedBy == u.Id select t).FirstOrDefault();
                    user.Add(new UserModel()
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Username = u.Username,
                        PurchasedTickets = trans == null ? new List<int>() : trans.TransportSchedules.Select(e => e.Id).ToList(),

                    });
                }

                return View(user);
            }
            return View();
        }






        public ActionResult UserDetails(int id)
        {

            var data = (from a in db.Users where a.Id == id select a).FirstOrDefault();
            var user = new UserModel();
            user.Id = data.Id;
            user.Name = data.Name;
            user.Username = data.Username;
            user.DateOfBirth = data.DateOfBirth;
            //user.CityName = data.City==null?"Undefined":data.City.Name;
            //user.CountryName = data.City == null ? "Undefined" : data.City.Country;
            user.Address = data.Address;
            user.Email = data.Email;
            user.Phone = data.Phone;

            return View(user);
        }



        public ActionResult PurchasedDetails(int id)
        {
            var purchasedetails = (from pd in db.PurchasedTickets where pd.PurchasedBy == id select pd).ToList();

            var pdetails = new List<PurchasedTicketModel>();

            foreach (var pd in purchasedetails)
            {
                var fromstopage = (from fs in db.Stopages where fs.Id == pd.FromStopageId select fs).FirstOrDefault();
                var fromstopagecity = (from fs in db.Cities where fs.Id == fromstopage.CityId select fs).FirstOrDefault();
                var tostopage = (from fs in db.Stopages where fs.Id == pd.ToStopageId select fs).FirstOrDefault();
                var tostopagecity = (from fs in db.Cities where fs.Id == tostopage.CityId select fs).FirstOrDefault();

                var seatInfo = (from si in db.SeatInfos where si.TicketId == pd.Id select si).FirstOrDefault();
                SeatClassEnum seatclass=null;
                AgeClassEnum ageclass=null;
                if (seatInfo!=null && seatInfo.SeatClass != null)
                {
                    seatclass = (from sc in db.SeatClassEnums where sc.Id == seatInfo.SeatClass select sc).FirstOrDefault();
                }
                if (seatInfo!=null && seatInfo.AgeClass != null)
                {
                    ageclass = (from ac in db.AgeClassEnums where ac.Id == seatInfo.AgeClass select ac).FirstOrDefault();
                }
                 //seatclass = (from sc in db.SeatClassEnums where sc.Id == seatInfo.SeatClass select sc).FirstOrDefault() ;
                 //ageclass = (from ac in db.AgeClassEnums where ac.Id == seatInfo.AgeClass select ac).FirstOrDefault();
                var trans = seatInfo==null?new Transport(): (from t in db.Transports where t.Id == seatInfo.TransportId select t).FirstOrDefault();
                var creator = (from cr in db.Users where cr.Id == trans.CreatedBy select cr.Name).FirstOrDefault();

                var FromRootFare = fromstopage.FareFromRoot;
                var ToRootFare = tostopage.FareFromRoot;
                var baseFare = Math.Abs((FromRootFare ?? -1) - (ToRootFare ?? -1));
                var cost = 0;
                if(seatclass!=null && seatclass.Value == "Business")
                {
                    cost = baseFare * 15;
                }
                else if (seatclass!=null && seatclass.Value == "Economic")
                {
                    cost = baseFare * 10;
                }
                else
                {
                    cost = baseFare * 12;
                }


                pdetails.Add(new PurchasedTicketModel()
                {
                    Id = pd.Id,
                    PurchasedBy = pd.PurchasedBy,
                    FromStopageName = fromstopage.Name,
                    FromStopageCityName = fromstopagecity.Name,
                    FromStopageCountryName = fromstopagecity.Country,
                    ToStopageName = tostopage.Name,
                    ToStopageCityName = tostopagecity.Name,
                    ToStopageCountryName = tostopagecity.Country,

                    StartTime = seatInfo==null?null: seatInfo.StartTime,
                    SeatNo = seatInfo == null ? null : seatInfo.SeatNo,
                    SeatClass = seatclass == null ? "Undefined" : seatclass.Value,
                    AgeClass = ageclass==null?"Undefined": ageclass.Value,
                    TransportCreatedBy = trans.CreatedBy,
                    TransportCreatorName= creator,
                    TransportName = trans==null?"Undefined": trans.Name,
                    TransportId = trans.Id,
                    Cost = cost
                    

                });
            }
            return View(pdetails);
        }


        public ActionResult CreatedSchedule(int id)
        {
            var trans = (from t in db.Transports where t.CreatedBy == id select t).ToList();

            var transports = new List<TransportModel>();
            foreach( var t in trans)
            {
                var transport = new TransportModel();
                transport.Id = t.Id;
                transport.Name = t.Name;
                transport.MaximumSeat = t.MaximumSeat;
                var transportSchedules = (from ts in db.TransportSchedules where ts.TransportId == t.Id select ts).ToList();

                //var schedules = new List<TransportScheduleModel>();
                foreach( var ts in transportSchedules) {
                    var fromstopage = (from fs in db.Stopages where fs.Id == ts.FromStopageId select fs).FirstOrDefault();
                    var fromstopagecity = (from fs in db.Cities where fs.Id == fromstopage.CityId select fs).FirstOrDefault();
                    var tostopage = (from fs in db.Stopages where fs.Id == ts.ToStopageId select fs).FirstOrDefault();
                    var tostopagecity = (from fs in db.Cities where fs.Id == tostopage.CityId select fs).FirstOrDefault();

                    var schedule = new TransportScheduleModel();
                    //schedules.Add(new TransportScheduleModel()
                    //{
                    schedule.Day = ts.Day;
                    schedule.FromStopageName = fromstopage.Name;
                    schedule.FromStopageCityName = fromstopagecity.Name;
                    schedule.FromStopageCountryName = fromstopagecity.Country;
                    schedule.ToStopageName = tostopage.Name;
                    schedule.ToStopageCityName = tostopagecity.Name;
                    schedule.ToStopageCountryName = tostopagecity.Country;
                    //});

                    transport.TransportSchedules.Add(schedule);

                }

                transports.Add(transport);

            }
            return View(transports);


        }


        

        public ActionResult TransportDetails(int id)
        {
            var transport = (from t in db.Transports where t.Id == id select t).FirstOrDefault();
            if (transport != null)
            {
                var schedule = (from sc in db.TransportSchedules where sc.TransportId == transport.Id select sc).ToList();
                var tdetails = new List<TransportScheduleModel>();
                foreach (var s in schedule)
                {
                    var fromstopage = (from fs in db.Stopages where fs.Id == s.FromStopageId select fs).FirstOrDefault();
                    var fromstopagecity = (from fs in db.Cities where fs.Id == fromstopage.CityId select fs).FirstOrDefault();
                    var tostopage = (from fs in db.Stopages where fs.Id == s.ToStopageId select fs).FirstOrDefault();
                    var tostopagecity = (from fs in db.Cities where fs.Id == tostopage.CityId select fs).FirstOrDefault();
                    //var fromstopage = (from fs in db.Cities where fs.Id == transport.FromStopageId select fs).FirstOrDefault();
                    //var tostopage = (from ts in db.Cities where ts.Id == transport.ToStopageId select ts).FirstOrDefault();
                    
                    

                    tdetails.Add(new TransportScheduleModel() 
                    {

                        Id = s.Id,
                        Day = s.Day,
                        Time = s.Time,
                        FromAirport = fromstopage.Name,
                        FromCity = fromstopagecity.Name,
                        FromCountry = fromstopagecity.Country,
                        ToAirport = tostopage.Name,
                        ToCity = tostopagecity.Name,
                        ToCountry = tostopagecity.Country,

                        MaximumSeat = transport.MaximumSeat,

                    });

                    
                }
                return View(tdetails);


            }
            return null;

        }
        [HttpPost]
        public ActionResult CancelTicket(int id, int tid)
        {
            
            //if (id == null) return RedirectToAction("PurchasedUserList");
            var tickets = (from t in db.PurchasedTickets where t.PurchasedBy == id select t).ToList();

            if(tickets.Count() > 1)
            {
                //var seat = (from s in db.SeatInfos where s.TicketId == id select s).FirstOrDefault();

                //if (seat != null)
                //{
                //    db.SeatInfos.Remove(seat);

                    //db.SaveChanges();
                //var ticket = (from t in db.PurchasedTickets where t.Id == tid select t).FirstOrDefault();
                //db.PurchasedTickets.Remove(ticket);

                //db.SaveChanges();

                var tickett = (from t in db.PurchasedTickets where t.Id == tid select t).FirstOrDefault();
                var seat = (from s in db.SeatInfos where s.TicketId == tid select s).FirstOrDefault();

                if (seat != null) db.SeatInfos.Remove(seat);

                //db.SaveChanges();
                if (tickett != null) db.PurchasedTickets.Remove(tickett);

                db.SaveChanges();

                TempData["msg"] = "Ticket Cancel Successfully";
                return RedirectToAction("PurchasedUserList");
            }

            TempData["msg"] = "Ticket Cannot be Canceled";
            return RedirectToAction("PurchasedUserList");


        }
        [HttpGet]
        public ActionResult AddUser()
        {
            return View(new UserModel());
        }

        [HttpPost]

        public ActionResult AddUser(UserModel u)
        {
            //if (u.Password.Equals(u.ConPassword))
            //{
                if (ModelState.IsValid)
                {
                    var user = new User()
                    {
                        Name = u.Name,
                        Username = u.Username,
                        Password = BCrypt.Net.BCrypt.HashPassword(u.Password, 12),
                        Address = u.Address,
                        DateOfBirth = u.DateOfBirth,
                        CityId = null,
                        FamilyId = null,
                        Email = u.Email,
                        Phone = u.Phone,
                        Role = u.Role
                    };
                    db.Users.Add(user);
                    db.SaveChanges();


                    TempData["msg"] = "User Account Created Successfully";
                    return RedirectToAction("SearchAllUsers", "Admin");
                }

            //}
            TempData["msg"] = "User Account Not Created";
            return View();
        }
        

        




        //////////////////// Evan End ////////////////////////

        //////////////////// Fahim Start ////////////////////////


        public ActionResult Userlist(string searching)
        {
            //Flight_ManagementEntities db = new Flight_ManagementEntities();
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
            var u = (from us in db.Users where us.Id == id select us).FirstOrDefault();

            var user = new UserModel()
            {
                Id = u.Id,
                Name = u.Name,
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
        [HttpPost]
        public ActionResult EditUserProfile(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var data = (from u in db.Users where u.Id == user.Id select u).FirstOrDefault();
                user.Username = data.Username;
                user.Password = data.Password;
                user.CityId = data.CityId;
                user.FamilyId = data.FamilyId;

                //user.Role = data.Role;
                //user.Email = data.Email;
                //user.Phone = data.Phone;

                db.Entry(data).CurrentValues.SetValues(user);
                db.SaveChanges();


                TempData["msg"] = "Profile Updated Successfully";
                return RedirectToAction("Userlist", "Admin");
            }
            //TempData["msg"] = "Information is not correct";
            return View(user);

        }
        public ActionResult DeleteUser(int id)
        {
            var user = (from u in db.Users where u.Id == id select u).FirstOrDefault();
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Userlist", "Admin");
        }
        
        public ActionResult Aircrafts(string searching)
        {
            var ac = db.Transports.ToList();
            if (!String.IsNullOrEmpty(searching))
            {
                ac = ac.Where(u => u.Name.Contains(searching)).ToList();
            }
            var acs = new List<TransportModel>();
            foreach (var a in ac)
            {
                acs.Add(new TransportModel()
                {
                    
                    Name = a.Name,
                   
                    MaximumSeat = a.MaximumSeat,
                    CreatorName = a.CreatedBy == null ? "undefined" : a.User.Name,
                    CreatedBy = a.CreatedBy
                });
            }
            return View(acs);
        }
        public ActionResult Flights(string searching)
        {
            //Flight_ManagementEntities db = new Flight_ManagementEntities();
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
                var schedule = (from fs in db.TransportSchedules
                                 where fs.TransportId == f.Id
                                 select fs).FirstOrDefault();
                
                if (schedule != null)
                {
                    flights.Add(new TransportModel()
                    {
                        Id = f.Id,
                        Name = f.Name,
                        From = (from t in db.Stopages where t.Id == schedule.FromStopageId select t.Name).FirstOrDefault(),
                        Destination = (from t in db.Stopages where t.Id == schedule.ToStopageId select t.Name).FirstOrDefault(),
                        MaximumSeat = f.MaximumSeat,
                        CreatorName = f.CreatedBy == null ? "undefined" : f.User.Name,
                        CreatedBy = f.CreatedBy,
                        AvailableSeats = availableSeats,
                        Day = schedule.Day,
                        Time = schedule.Time/100
                    });
                } 
                
            }
            return View(flights);
        }
        public ActionResult BookedSeatsFlights(int id)
        {
            Flight_ManagementEntities db = new Flight_ManagementEntities();
            var seat = (from s in db.SeatInfos where s.TransportId == id && s.Status == "Booked" select s).ToList();

            var seats = new List<SeatInfosModel>();
            foreach (var s in seat)
            {
                seats.Add(new SeatInfosModel()
                {
                    //Id = s.Id,
                    SeatNo = s.SeatNo,
                    SeatClassName = s.SeatClass == null ? "undefined" : s.SeatClassEnum.Value,
                    PurchasedById = s.TicketId == null ? 0 : s.PurchasedTicket.PurchasedBy,
                    PurchasedByName = s.TicketId == null ? "undefined" : s.PurchasedTicket.User.Name,
                    TicketId = s.TicketId == null ? 0 : s.TicketId,
                    Status = s.Status,
                    AircraftName = s.Transport.Name,
                });
            }

            return View(seats);
        }
        public ActionResult TicketCancel(int? id)
        {
            if (id == null) return RedirectToAction("Flights");
            var ticket = (from t in db.PurchasedTickets where t.Id == id select t).FirstOrDefault();
            var seat = (from s in db.SeatInfos where s.TicketId == id select s).FirstOrDefault();

            if (seat != null) db.SeatInfos.Remove(seat);

            //db.SaveChanges();
            if (ticket != null) db.PurchasedTickets.Remove(ticket);

            db.SaveChanges();

            //TempData["msg"] = "Ticket Cancel Successfully";
            return RedirectToAction("Flights");




        }




        //[HttpGet]
        //public ActionResult SearchFlight()
        //{
        //    var data = (from cs in db.Cities select cs).ToList();
        //    var cities = new List<CityModel>();

        //    foreach(var c in data)
        //    {
        //        cities.Add(new CityModel()
        //        {
        //            Name = c.Name,
        //        });
        //    }

        //    return View(cities);
        //}
        //[HttpPost]
        //public ActionResult SearchFlight(string Date, string From, string Destination)
        //{

        //    var cities = (from cs in db.Cities select cs);
        //    if (From != null && Destination != null)
        //    {
        //        var fromcity = cities.Where(cs => cs.Name.Contains(From)).FirstOrDefault();
        //        var destinationcity = cities.Where(cs => cs.Name.Contains(Destination)).FirstOrDefault();
        //        var flights = (from fs in db.TransportSchedules where fs.FromStopageId == fromcity.Id 
        //                       && fs.ToStopageId == destinationcity.Id && fs.Day.Equals(Date) select fs).FirstOrDefault();

        //        if(flights != null)
        //        {
        //            var transports = (from t in db.Transports where t.Id == flights.TransportId select t).FirstOrDefault();
        //            var trans = new TransportModel()
        //            {
        //                Name = transports.Name,
        //                Id = flights.TransportId,
        //            };
        //            return View(trans);

        //        }


        //    }

        //    return View();
        //}


        //////////////////// Fahim End ////////////////////////




    }
}