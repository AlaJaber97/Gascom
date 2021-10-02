using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL.Utils
{
    public static class JwtConfig
    {
        public static string JwtKey => "SOME_RANDOM_KEY_DO_NOT_SHARE";
        public static string JwtIssuer => BLL.Settings.Configration.ApiServerAddress;
        public static int JwtExpireDays => 30;
    }
    public static class Jwt
    {
        public static string GenerateJwtToken(string email, IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.JwtKey));
            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(JwtConfig.JwtExpireDays);

            var token = new JwtSecurityToken(JwtConfig.JwtIssuer, JwtConfig.JwtIssuer, claims, expires: expires, signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
