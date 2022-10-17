using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Service.IService.UserInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO.User;
using BookingSoccers.Service.Models.Payload.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace BookingSoccers.Controllers.UserInfo
{
    [Route("api/v1/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly BookingSoccersContext bookingSoccersContext;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UsersController(BookingSoccersContext bookingSoccersContext,
            IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.bookingSoccersContext = bookingSoccersContext;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {

            var result = await userService.RetrieveAllUsers();

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> AddNewUserForAdmin(UserCreatePayload userInfo)
        {
            var AddedUser = await userService.AddANewUser(userInfo);

            if (AddedUser.IsSuccess)
                return Ok(AddedUser);

            Response.StatusCode = AddedUser.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedUser);

            return StatusCode(AddedUser.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddNewUserNormal(UserCreatePayload userInfo)
        {
            var AddedUser = await userService.AddANewUser(userInfo);

            if (AddedUser.IsSuccess)
                return Ok(AddedUser);

            Response.StatusCode = AddedUser.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedUser);

            return StatusCode(AddedUser.StatusCode, response);
        }

        [Authorize(Roles ="Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAUser(int id, UserUpdatePayload NewUserInfo)
        {


            var updatedUser = await userService.UpdateAUser(id, NewUserInfo);

            if (updatedUser.IsSuccess)
                return Ok(updatedUser);

            Response.StatusCode = updatedUser.StatusCode;

            var response = mapper.Map<ErrorResponse>(updatedUser);

            return StatusCode(updatedUser.StatusCode, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneSpecificUser(int id)
        {
            var retrievedUser = await userService.RetrieveAUserById(id);

            if (retrievedUser.IsSuccess)
                return Ok(retrievedUser);

            Response.StatusCode = retrievedUser.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedUser);

            return StatusCode(retrievedUser.StatusCode, response);
        }

        [HttpGet("{username}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserForUserUpdate(string Username) 
        {
            var retrievedUser = await userService.RetrieveAUserForUpdate(Username);

            if (retrievedUser.IsSuccess)
                return Ok(retrievedUser);

            Response.StatusCode = retrievedUser.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedUser);

            return StatusCode(retrievedUser.StatusCode, response);
        }

        [HttpPut("user/{id}")]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> UpdateUserInfoForUser(int id, UserUpdatePayload toUpdateInfo) 
        {

            var retrievedUser = await userService.UpdateUserInfoForUser(id,toUpdateInfo);

            if (retrievedUser.IsSuccess)
                return Ok(retrievedUser);

            Response.StatusCode = retrievedUser.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedUser);

            return StatusCode(retrievedUser.StatusCode, response);
        }


         [Authorize(Roles ="Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAUser(int id)
        {
            var deletedUser = await userService.RemoveAUser(id);

            if (deletedUser.IsSuccess)
                return Ok(deletedUser);

            Response.StatusCode = deletedUser.StatusCode;

            var response = mapper.Map<ErrorResponse>(deletedUser);

            return StatusCode(deletedUser.StatusCode, response);
        }
    }
}
