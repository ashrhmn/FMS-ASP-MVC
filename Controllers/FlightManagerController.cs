using Flight_Management_System.Auth;
using Flight_Management_System.Models.AuthEntities;
using Flight_Management_System.Models.Database;
using Flight_Management_System.Models.FlightManagerEntities;
using Flight_Management_System.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flight_Management_System.Controllers
{
    public class FlightManagerController : Controller
    {
        private Flight_ManagementEntities db;
        private JwtManage jwt;
        public FlightManagerController()
        {
            db = new Flight_ManagementEntities();
            jwt = new JwtManage();
        }
        [FlightManagerAccess]
        [HttpGet]
        public ActionResult Dashboard()
        {
            AuthPayload user = jwt.LoggedInUser(Request.Cookies);
            var dbAirlines = db.Transports.Where(t => t.CreatedBy == user.Id).ToList();
            List<AirlineModel> airlines = new List<AirlineModel>();
            foreach (var airline in dbAirlines)
            {
                AirlineModel airlineModel = new AirlineModel()
                {
                    Id = airline.Id,
                    Name = airline.Name,
                    FromStopageId = airline.FromStopageId,
                    FromAirport = airline.Stopage == null ? "Undefined" : airline.Stopage.Name,
                    ToAirport = airline.Stopage1 == null ? "Undefined" : airline.Stopage1.Name,
                    FromCity = airline.Stopage==null?"Undefined": airline.Stopage.City.Name,
                    FromCountry = airline.Stopage == null ? "Undefined" : airline.Stopage.City.Country,
                    ToStopageId =  airline.ToStopageId,
                    ToCity = airline.Stopage1 == null ? "Undefined" : airline.Stopage1.City.Name,
                    ToCountry = airline.Stopage1 == null ? "Undefined" : airline.Stopage1.City.Country,
                    SeatCapacity = airline.MaximumSeat ?? 0
                };
                airlines.Add(airlineModel);
            }
            return View(airlines);
        }

        [FlightManagerAccess]
        [HttpGet]
        public ActionResult AddAircraft()
        {
            return View(new AirlineModel());
        }

        [FlightManagerAccess]
        [HttpPost]
        public ActionResult AddAircraft(AirlineModel airline)
        {
            if (ModelState.IsValid)
            {
                AuthPayload user = jwt.LoggedInUser(Request.Cookies);
                if (user == null) return RedirectToAction("SignIn", "Auth");
                Transport transport = new Transport()
                {
                    Name = airline.Name,
                    FromStopageId = airline.FromStopageId,
                    ToStopageId = airline.ToStopageId,
                    MaximumSeat = airline.SeatCapacity,
                    CreatedBy = user.Id
                };
                db.Transports.Add(transport);
                db.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View(airline);
            }

        }
    }
}