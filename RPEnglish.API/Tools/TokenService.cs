using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RPEnglish.API.Entities;

namespace RPEnglish.API.Tools
{
    public static class TokenService
    {
        public static string SecretKey = "6717883c-60e1-4084-a592-e35efb6814fa";

        public static object GenerateToken(User user)
        {
            DateTime expireDate = DateTime.Now.AddHours(12);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(TokenService.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, "Default"),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                NotBefore = DateTime.Now,
                Expires = expireDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return new { token = tokenHandler.WriteToken(token), Expiration = expireDate };
        }
    }
}