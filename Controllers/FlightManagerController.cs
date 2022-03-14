﻿using Flight_Management_System.Auth;
using Flight_Management_System.Models.AuthEntities;
using Flight_Management_System.Models.Database;
using Flight_Management_System.Models.FlightManagerEntities;
using Flight_Management_System.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Flight_Management_System.Controllers
{
    [FlightManagerAccess]
    public class FlightManagerController : Controller
    {
        private readonly Flight_ManagementEntities _db;
        private readonly JwtManage _jwt;
        public FlightManagerController()
        {
            _db = new Flight_ManagementEntities();
            _jwt = new JwtManage();
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            AuthPayload user = _jwt.LoggedInUser(Request.Cookies);
            var dbAirlines = _db.Transports.Where(t => t.CreatedBy == user.Id).ToList();
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
                var airlineModel = new AirlineModel()
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



        [HttpGet]
        public ActionResult AddAircraftSchedule(int? id)
        {
            if (id == null) return RedirectToAction("Dashboard");
            var aircraft = _db.Transports.FirstOrDefault(t => t.Id == id);
            if (aircraft == null) return RedirectToAction("Dashboard");
            return View(new TransportScheduleModel() { TransportId = aircraft.Id });
        }

        [HttpPost]
        public ActionResult AddAircraftSchedule(int? id, TransportScheduleModel transportScheduleModel)
        {

            if (id == null) return RedirectToAction("Dashboard");
            if (transportScheduleModel.FromStopageId == transportScheduleModel.ToStopageId) return View(transportScheduleModel);
            var aircraft = _db.Transports.FirstOrDefault(t => t.Id == id);
            if (aircraft == null) return RedirectToAction("Dashboard");
            TransportSchedule schedule = new TransportSchedule()
            {
                TransportId = aircraft.Id,
                FromStopageId = transportScheduleModel.FromStopageId,
                ToStopageId = transportScheduleModel.ToStopageId,
                Time = int.Parse(transportScheduleModel.TimeH.ToString() + transportScheduleModel.TimeM.ToString()),
                Day = transportScheduleModel.Day,
            };
            _db.TransportSchedules.Add(schedule);
            _db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public ActionResult DeleteAircraftSchedule(int? id)
        {
            if (id == null) return RedirectToAction("Dashboard");
            var schedule = _db.TransportSchedules.FirstOrDefault(t => t.Id == id);
            if (schedule == null) return RedirectToAction("Dashboard");
            _db.TransportSchedules.Remove(schedule);
            _db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public ActionResult DeleteAircraft(int? id)
        {
            if (id == null) return RedirectToAction("Dashboard");
            var schedules = _db.TransportSchedules.Where((t) => t.TransportId == id).ToList();
            var aircraft = _db.Transports.FirstOrDefault(t => t.Id == id);
            _db.TransportSchedules.RemoveRange(schedules);
            if (aircraft != null) _db.Transports.Remove(aircraft);
            _db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public ActionResult AddAircraft(AirlineModel airline)
        {
            if (!ModelState.IsValid) return View(airline);
            var user = _jwt.LoggedInUser(Request.Cookies);
            if (user == null) return RedirectToAction("SignIn", "Auth");

            var transportSchedules = airline.TransportSchedules.Select(transportScheduleModel => new TransportSchedule()
            {
                Time = transportScheduleModel.Time,
                Day = transportScheduleModel.Day,
                FromStopageId = transportScheduleModel.FromStopageId,
                ToStopageId = transportScheduleModel.ToStopageId

            }
            ).ToList();

            var transport = new Transport()
            {
                Name = airline.Name,
                TransportSchedules = transportSchedules,
                MaximumSeat = airline.SeatCapacity,
                CreatedBy = user.Id
            };
            _db.Transports.Add(transport);
            _db.SaveChanges();
            return RedirectToAction("Dashboard");

        }
    }
}