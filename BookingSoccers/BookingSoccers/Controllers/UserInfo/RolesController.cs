using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Service.IService.UserInfo;
using BookingSoccers.Service.UserInfo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingSoccers.Controllers.UserInfo
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly BookingSoccersContext bookingSoccersContext;
        private readonly IRoleService roleService;

        public RolesController(BookingSoccersContext bookingSoccersContext, IRoleService roleService)
        {
            this.roleService = roleService;
            this.bookingSoccersContext = bookingSoccersContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            if (bookingSoccersContext.Roles == null) return NotFound();
            var result = await roleService.RetrieveAllRoles();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRole(string RoleName)
        {
            if (!ModelState.IsValid) return BadRequest();

            Role AddedRole = await roleService.AddANewRole(RoleName);
            if (AddedRole != null) return Ok(AddedRole);
            return BadRequest(AddedRole);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateARole(int id, string NewRoleName)
        {
            if (!ModelState.IsValid) return BadRequest();

            Role updatedRole = await roleService.UpdateARole(id, NewRoleName);
            if (updatedRole != null) return Ok(updatedRole);
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneSpecificRole(int id)
        {
            Role retrievedRole = await roleService.RetrieveARoleById(id);
            if (retrievedRole != null) return Ok(retrievedRole);
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteARole(int id)
        {
            Role deletedRole = await roleService.RemoveARole(id);
            if (deletedRole != null) return Ok(deletedRole);
            return NotFound();
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
