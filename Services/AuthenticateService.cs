using Hubtel.Wallets.Api.Data;
using Hubtel.Wallets.Api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hubtel.Wallets.Api.Services
{
    public class AuthenticateService:IAuthenticateService

    {
        private readonly JwtConfig _jwtConfig;

        private readonly UserServices _userServices;
        public AuthenticateService(IOptions<JwtConfig> jwtConfig,ApplicationDbContext dbContext)
        {
            _jwtConfig = jwtConfig.Value;
            _userServices = new UserServices(dbContext);
        }

        public User AuthenticateUser(string email, string newPassword)
        {
            User user = _userServices.GetUser(email);

            // return null if user is not found
            if (user == null)
                return null;

            // Verify the entered password
            if (BCrypt.Net.BCrypt.Verify(newPassword, user.Password) == false)
            {
                return null;
            }

            // if user is found
            var tokenHandler = new JwtSecurityTokenHandler();
            var SecretKey = Encoding.ASCII.GetBytes(_jwtConfig.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Name)
                }),

                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(SecretKey),SecurityAlgorithms.HmacSha256Signature)    

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            user.Password = null;

            return user; 

        }

    }
}
