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
    [FlightManagerAccess]
    public class FlightManagerController : Controller
    {
        private Flight_ManagementEntities db;
        private JwtManage jwt;
        public FlightManagerController()
        {
            db = new Flight_ManagementEntities();
            jwt = new JwtManage();
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            AuthPayload user = jwt.LoggedInUser(Request.Cookies);
            var dbAirlines = db.Transports.Where(t => t.CreatedBy == user.Id).ToList();
            List<AirlineModel> airlines = new List<AirlineModel>();
            foreach (var airline in dbAirlines)
            {
                List<TransportScheduleModel> transportSchedules = new List<TransportScheduleModel>();
                foreach (var transportSchedule in airline.TransportSchedules)
                {
                    transportSchedules.Add(new TransportScheduleModel()
                    {
                        Id = transportSchedule.Id,
                        Day = transportSchedule.Day,
                        Time = transportSchedule.Time ?? 0,
                        FromStopageId = transportSchedule.FromStopageId,
                        FromAirport = transportSchedule.FromStopageId == null ? "Undefined" : transportSchedule.Stopage.Name,
                        FromCity = transportSchedule.FromStopageId == null ? "Undefined" : transportSchedule.Stopage.City.Name,
                        FromCountry = transportSchedule.FromStopageId == null ? "Undefined" : transportSchedule.Stopage.City.Country,
                        ToStopageId = transportSchedule.ToStopageId,
                        ToAirport = transportSchedule.ToStopageId == null ? "Undefined" : transportSchedule.Stopage1.Name,
                        ToCity = transportSchedule.ToStopageId == null ? "Undefined" : transportSchedule.Stopage1.City.Name,
                        ToCountry = transportSchedule.ToStopageId == null ? "Undefined" : transportSchedule.Stopage1.City.Country,
                    });
                }
                AirlineModel airlineModel = new AirlineModel()
                {
                    Id = airline.Id,
                    Name = airline.Name,
                    TransportSchedules = transportSchedules,
                    SeatCapacity = airline.MaximumSeat ?? 0
                };
                airlines.Add(airlineModel);
            }
            return View(airlines);
        }

        [HttpGet]
        public ActionResult AddAircraft()
        {
            return View(new AirlineModel());
        }



        [HttpGet] public ActionResult AddAircraftSchedule(int? id)
        {
            if (id == null) return RedirectToAction("Dashboard");
            var aircraft = db.Transports.FirstOrDefault(t => t.Id == id);
            if (aircraft == null) return RedirectToAction("Dashboard");
            //TransportScheduleModel scheduleModel = new TransportScheduleModel() { TransportId=aircraft.Id };
            //List<TransportScheduleModel> schedules = new List<TransportScheduleModel>();
            //foreach (var item in aircraft.TransportSchedules)
            //{
            //    schedules.Add(new TransportScheduleModel()
            //    {
            //        Id = item.Id,
            //        Day = item.Day,
            //        Time = item.Time ?? 0,
            //        FromStopageId = item.FromStopageId,
            //        FromAirport = item.Stopage.Name,
            //        FromCity = item.Stopage == null ? "Undefined" : item.Stopage.City.Name,
            //        FromCountry = item.Stopage == null ? "Undefined" : item.Stopage.City.Country,
            //        ToStopageId = item.ToStopageId,
            //        ToAirport = item.Stopage1.Name,
            //        ToCity = item.Stopage1 == null ? "Undefined" : item.Stopage1.City.Name,
            //        ToCountry = item.Stopage1 == null ? "Undefined" : item.Stopage1.City.Country,
            //    });
            //}
            return View(new TransportScheduleModel() { TransportId = aircraft.Id });
        }

        [HttpPost]
        public ActionResult AddAircraftSchedule(int? id, TransportScheduleModel transportScheduleModel)
        {

            if (id == null) return RedirectToAction("Dashboard");
            var aircraft = db.Transports.FirstOrDefault(t => t.Id == id);
            if (aircraft == null) return RedirectToAction("Dashboard");
            TransportSchedule schedule = new TransportSchedule() 
            { 
                TransportId = aircraft.Id,
                FromStopageId = transportScheduleModel.FromStopageId,
                ToStopageId = transportScheduleModel.ToStopageId,
                Time = Int32.Parse(transportScheduleModel.TimeH.ToString() + transportScheduleModel.TimeM.ToString()),
                Day = transportScheduleModel.Day,
            };
            db.TransportSchedules.Add(schedule);
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public ActionResult DeleteAircraftSchedule(int? id)
        {
            if (id!=null)
            {
                var schedule = db.TransportSchedules.Where(t => t.Id == id).FirstOrDefault();
                if (schedule != null)
                {
                    db.TransportSchedules.Remove(schedule);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public ActionResult DeleteAircraft(int? id)
        {
            if (id != null)
            {
                var schdules = db.TransportSchedules.Where((t) => t.TransportId == id).ToList();
                var aircraft = db.Transports.Where(t=>t.Id==id).FirstOrDefault();
                db.TransportSchedules.RemoveRange(schdules);
                db.Transports.Remove(aircraft);
                db.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public ActionResult AddAircraft(AirlineModel airline)
        {
            if (ModelState.IsValid)
            {
                AuthPayload user = jwt.LoggedInUser(Request.Cookies);
                if (user == null) return RedirectToAction("SignIn", "Auth");

                List<TransportSchedule> transportSchedules = new List<TransportSchedule>();
                foreach (TransportScheduleModel transportScheduleModel in airline.TransportSchedules)
                {
                    TransportSchedule transportSchedule = new TransportSchedule()
                    {
                        Time = transportScheduleModel.Time,
                        Day = transportScheduleModel.Day,
                        FromStopageId = transportScheduleModel.FromStopageId,
                        ToStopageId = transportScheduleModel.ToStopageId
                    };
                    transportSchedules.Add(transportSchedule);
                }

                Transport transport = new Transport()
                {
                    Name = airline.Name,
                    TransportSchedules = transportSchedules,
                    //FromStopageId = airline.FromStopageId,
                    //ToStopageId = airline.ToStopageId,
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