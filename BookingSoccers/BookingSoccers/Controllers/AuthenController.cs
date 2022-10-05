
using Microsoft.AspNetCore.Mvc;

using BookingSoccers.Service.IService;
using BookingSoccers.Repo.Context;
using AutoMapper;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload;
namespace BookingSoccers.Controllers
{
    [Route("api/authen")]
    [ApiController]

    public class AuthenController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAuthenService authenService;

        public AuthenController(IMapper mapper,
            IAuthenService authenService)
        {
            this.mapper = mapper;
            this.authenService = authenService;
        }

        [HttpGet("/login")]
        public async Task<IActionResult> Login(string AccessToken)
        {
            var result =  await authenService.Authentication(AccessToken);

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }

        [HttpPost("/refresh-token")]
        public async Task<IActionResult> RefreshAccessToken(TokenRefresh refreshTokenInfo)
        {
            var result = await authenService.RefreshToken(refreshTokenInfo);

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }
    }
}
