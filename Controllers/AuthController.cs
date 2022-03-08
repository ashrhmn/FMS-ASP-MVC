using Flight_Management_System.Models;
using Flight_Management_System.Models.Database;
using JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flight_Management_System.Controllers
{
    public class AuthController : Controller
    {
        private Flight_ManagementEntities db;
        public AuthController()
        {
            db = new Flight_ManagementEntities();
        }
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public User SignUp(UserModel userModel)
        {
            var existingUser = db.Users.FirstOrDefault(u => u.Username == userModel.Username);
            if (existingUser == null)
            {
                var user = new User()
                {
                    Name = userModel.Name,
                    Username = userModel.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password, 12),
                    Address = userModel.Address,
                    DateOfBirth = userModel.DateOfBirth,
                    CityId = userModel.CityId,
                    FamilyId = userModel.FamilyId,
                    Role = userModel.Role,
                };
                db.Users.Add(user);
                db.SaveChanges();
                return user;
            }
            else
            {
                return null;
            }
        }


        [HttpPost]
        public string Login(UserModel userModel)
        {
            var user = db.Users.Where(u => u.Username == userModel.Username).First();

            bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(userModel.Password, user.Password);

            if (isCorrectPassword)
            {
                const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

                JWT.Algorithms.IJwtAlgorithm algorithm = new JWT.Algorithms.HMACSHA256Algorithm();
                JWT.IJsonSerializer serializer = new JWT.Serializers.JsonNetSerializer();
                JWT.IBase64UrlEncoder urlEncoder = new JWT.JwtBase64UrlEncoder();
                JWT.IJwtEncoder encoder = new JWT.JwtEncoder(algorithm, serializer, urlEncoder);

                var token = encoder.Encode(user, secret);
                return token;
            }

            return null;

        }


        [HttpPost]
        public string CurrentUser(string token)
        {
            const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

            try
            {
                IJsonSerializer serializer = new JWT.Serializers.JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                JWT.Algorithms.IJwtAlgorithm algorithm = new JWT.Algorithms.HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var json = decoder.Decode(token, secret, verify: true);
                return json;
            }
            catch (JWT.Exceptions.TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
                return null;
            }
            catch (JWT.Exceptions.SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
                return null;
            }
        }

        // GET: Auth/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Auth/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Auth/Create
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

        // GET: Auth/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Auth/Edit/5
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

        // GET: Auth/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Auth/Delete/5
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
    }
}
