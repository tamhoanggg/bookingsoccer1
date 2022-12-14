using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Service.IService.UserInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.User;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingSoccers.Controllers.UserInfo
{
    [Route("api/v1/roles")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly BookingSoccersContext bookingSoccersContext;
        private readonly IRoleService roleService;
        private readonly IMapper mapper;

        public RolesController(BookingSoccersContext bookingSoccersContext,
            IRoleService roleService, IMapper mapper)
        {
            this.roleService = roleService;
            this.bookingSoccersContext = bookingSoccersContext;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        //Get all roles
        public async Task<IActionResult> GetRoles()
        {

            var result = await roleService.RetrieveAllRoles();
            
            if(result.IsSuccess)
            return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("role-details")]
        //Get details of a role
        public async Task<IActionResult> GetARoleDetails
            ([FromQuery]PagingPayload pagingPayload, [FromQuery]UserPredicate filter)
        {
            var retrievedRole = await roleService.RetrieveARoleDetails
                (pagingPayload, filter);

            if (retrievedRole.IsSuccess)
                return Ok(retrievedRole);

            Response.StatusCode = retrievedRole.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedRole);

            return StatusCode(retrievedRole.StatusCode, response);
        }

        [HttpGet("{id}/users")]
        [Authorize(Roles = "Admin")]
        //Get users of a role
        public async Task<IActionResult> GetUsersOfARole(byte id) 
        {
            var result = await roleService.GetUsersOfARole(id);

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }

         [Authorize(Roles ="Admin")]
        [HttpPost]
        //Add a new role
        public async Task<IActionResult> AddNewRole(string RoleName)
        {
            var AddedRole = await roleService.AddANewRole(RoleName);
            if (AddedRole.IsSuccess) 
                return Ok(AddedRole);

            Response.StatusCode = AddedRole.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedRole);

            return StatusCode(AddedRole.StatusCode, response);
        }
         [Authorize(Roles ="Admin")]
        [HttpPut("{id}")]
        //Update an existing role
        public async Task<IActionResult> UpdateARole(byte id, string NewRoleName)
        {
            

            var updatedRole = await roleService.UpdateARole(id, NewRoleName);

            if (updatedRole.IsSuccess)
                return Ok(updatedRole);

            Response.StatusCode = updatedRole.StatusCode;

            var response = mapper.Map<ErrorResponse>(updatedRole);

            return StatusCode(updatedRole.StatusCode, response);
        }

         [Authorize(Roles ="Admin")]
        [HttpDelete("{id}")]
        //Delete a role
        public async Task<IActionResult> DeleteARole(byte id)
        {
            var deletedRole = await roleService.RemoveARole(id);

            if (deletedRole.IsSuccess)
                return Ok(deletedRole);

            Response.StatusCode = deletedRole.StatusCode;

            var response = mapper.Map<ErrorResponse>(deletedRole);

            return StatusCode(deletedRole.StatusCode, response);
        }

        //[HttpDelete]
        //public async Task<IActionResult> DeleteAllRoles() 
        //{ 
        //    List<Role> deletedRoleList = await roleService.RemoveAllRoles();
        //    if(deletedRoleList != null) return Ok(deletedRoleList);
        //    return NotFound();
        //}
    }
}
