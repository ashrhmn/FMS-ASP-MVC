using JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Flight_Management_System.Models.AuthEntities;

namespace Flight_Management_System.Utils
{
    public class JwtManage
    {
        private static string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

        public string EncodeToken(Object payload)
        {
            JWT.Algorithms.IJwtAlgorithm algorithm = new JWT.Algorithms.HMACSHA256Algorithm();
            JWT.IJsonSerializer serializer = new JWT.Serializers.JsonNetSerializer();
            JWT.IBase64UrlEncoder urlEncoder = new JWT.JwtBase64UrlEncoder();
            JWT.IJwtEncoder encoder = new JWT.JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, secret);
            return token;
        }

        public string DecodeToken(string token)
        {
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
            catch (Exception error)
            {
                Console.WriteLine("JWT Decode error : "+error.ToString());
                return null;
            }
        }

        public AuthPayload LoggedInUser(HttpCookieCollection cookies)
        {
            HttpCookie cookie = cookies["session"];
            if (cookie == null) return null;
            var token = cookie["token"];
            var decodedObject = DecodeToken(token);
            if (decodedObject == null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookies.Add(cookie);
                return null;
            }
            var result = JsonConvert.DeserializeObject<AuthPayload>(decodedObject);
            return result;
        }

        public bool DeleteToken(HttpCookieCollection cookies)
        {
            HttpCookie cookie = cookies["session"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookies.Add(cookie);
            }
            return true;
        }
    }
}