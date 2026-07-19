using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {

           //private claims
           var authClaims = new List<Claim>
           {
               new Claim(ClaimTypes.GivenName, user.UserName),
               new Claim(ClaimTypes.Email, user.Email)
           };
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var Role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, Role));
            }
            //JWT Signature(secret key)
            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            //register claims(public)
            var token = new JwtSecurityToken(
                audience: _configuration["Jwt:ValidAudience"],
                issuer: _configuration["Jwt:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                claims:authClaims,
                signingCredentials:new SigningCredentials(authkey,SecurityAlgorithms.HmacSha256Signature)

                ) ;



            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
