using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TrackerAPI.Helper;

namespace TrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private TrackerDAL.TrackerSystem _trackerSystem;
        public AuthController(ConnectionStrings connectionStrings)
        {
            _trackerSystem = new TrackerDAL.TrackerSystem(connectionStrings.GetConnectionString());
        }

        [HttpPost, Route("[action]")]
        public async Task<IActionResult> Login([FromBody] Models.Auth auth) 
        {
            if (!ModelState.IsValid) return StatusCode(400, ModelState);
            TrackerDAL.Models.Users user =  await _trackerSystem.Login(auth.Email, auth.Password);
            if(user != null)
            {
                string jti = Guid.NewGuid().ToString();
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("userID", user.UsersID.ToString()));
                claims.Add(new Claim("name", user.Name));
                claims.Add(new Claim("email", user.Email));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));
                string token = new JwtSecurityTokenHandler().WriteToken(await CreateToken("localhost", "localhost", claims, 14400, "Localhost.2020*****"));
                return StatusCode(200, new
                {
                    token
                });
            }
            return StatusCode(404);
        }
        
        private async Task<JwtSecurityToken> CreateToken(string issuer, string audience, List<Claim> claims, double expirationMinutes, string securityKey)
        {
            return await Task.Run(() => new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expirationMinutes),
                notBefore: DateTime.Now,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)), SecurityAlgorithms.HmacSha256)
                ));
        }
    }
}
