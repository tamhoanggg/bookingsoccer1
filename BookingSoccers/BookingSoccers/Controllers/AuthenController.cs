
using Microsoft.AspNetCore.Mvc;

using BookingSoccers.Service.IService;
using BookingSoccers.Repo.Context;


namespace BookingSoccers.Controllers
{
    [Route("api/authen")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly BookingSoccersContext bookingSoccersContext;
        private readonly IAuthenService authenService;

        public AuthenController(BookingSoccersContext bookingSoccersContext,
            IAuthenService authenService)
        {
            this.bookingSoccersContext = bookingSoccersContext;
            this.authenService = authenService;
        }

        [HttpGet("/login")]
        public async Task<IActionResult> Login(string AccessToken)
        {
            var result =  await authenService.Authentication(AccessToken);
            if (result == null) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("/refresh-token")]
        public async Task<IActionResult> RefreshAccessToken(string accessToken, string refreshToken, 
            DateTime refreshTokenExpiryDate)
        {
            var result = await authenService.RefreshToken(accessToken, refreshToken, refreshTokenExpiryDate);
            if (result == null) return Unauthorized();
            return Ok(result);
        }
    }
}
