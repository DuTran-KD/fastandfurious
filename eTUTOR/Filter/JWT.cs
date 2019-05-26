using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace eTUTOR.Filter
{
    public class JWT
    {
        public string TokenStream(JSONObject data)
        {
            string api_key = "apiKey123123";
            string api_secret = "MYCUSTOMCODELONGMOD4NEEDBEZE";

            var signingKey = Convert.FromBase64String(api_secret);

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(signingKey);
            JwtHeader jwtHeader = new JwtHeader(
               new SigningCredentials(
                   securityKey,
                   SecurityAlgorithms.HmacSha512,
                   SecurityAlgorithms.RsaSha384
               )
            );

            JwtPayload jwtPayload = new JwtPayload {
                {"iss", api_key},
                {"aud", api_key},
                {"sub", api_key},
                {"data", data},
                { "exp", ((DateTimeOffset)DateTime.UtcNow).AddHours(3).ToUnixTimeSeconds() }
            };

            var jwt = new JwtSecurityToken(jwtHeader,jwtPayload);
            var jwtHandler = new JwtSecurityTokenHandler();
            string tokenJWT = jwtHandler.WriteToken(jwt);
            string newTokenHeader = "eyJhbG" + Base64Encode(api_key);
            string newTokenPayload = ".eyJpc3Mi" + RandomString(16);
            string token = tokenJWT.Replace("eyJhbG", newTokenHeader).Replace(".eyJpc3Mi", newTokenPayload);
            return token;
        }

        public JSONObject DataJSON(dynamic data)
        {
            var jsonData = new JSONObject
            {
                email = data.email,
                fullname = data.fullname,
                username = data.username,
                status = data.status,
                avatar = data.avatar,
                phone = data.phone
            };
            return jsonData;
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}