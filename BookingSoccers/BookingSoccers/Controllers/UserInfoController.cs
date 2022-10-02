using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace BookingSoccers.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private IConfiguration _config;

        public UserInfoController(IConfiguration config)
        {
            _config = config;
        }

        [Authorize]
        [HttpGet("/login")]
        public async Task<String> Login () 
        {
            var UserClaims = User.Claims;
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JwtToken:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(UserClaims),
                Issuer = _config["JwtToken:Issuer"],
                Audience = _config["JwtToken:Audience"],
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            //Console.WriteLine(UserClaims);
            //Console.WriteLine(tokenString);
            Console.WriteLine("Called get Data");
            return tokenString;
        }

    }
}
