using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
using BookingSoccers.Repo.Repository.UserInfo;
using BookingSoccers.Service.IService.UserInfo;
using BookingSoccers.Service.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace BookingSoccers.Service.UserInfo
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepo roleRepo;
        //private readonly IMapper mapper;

        public RoleService(IRoleRepo roleRepo/*, IMapper mapper*/)
        {
            this.roleRepo = roleRepo;
            //this.mapper = mapper;
        }

        public async Task<GeneralResult<Role>> AddANewRole(string roleName)
        {
            var existRole = await roleRepo.GetRoleByName(roleName);
            if (existRole != null) return GeneralResult<Role>.Error(403 , "Role already exists");

            Role newRoleObj = new Role()
            {
                Name = roleName
            };
            roleRepo.Create(newRoleObj);
            await roleRepo.SaveAsync();

            return GeneralResult<Role>.Success(newRoleObj);
        }

        public async Task<GeneralResult<Role>> RetrieveARoleById(byte roleId)
        {
            var returnedRole = await roleRepo.GetById(roleId);
            if (returnedRole == null) return GeneralResult<Role>.Error(
                204 , "Role not found with Id:"+roleId);
            return GeneralResult<Role>.Success(returnedRole);
        }

        public async Task<GeneralResult < List<Role> > > RetrieveAllRoles() 
        { 
            var roles = await roleRepo.Get().ToListAsync();
            if (roles == null) return GeneralResult< List<Role> >.Error(
                204, "No role found");
            return GeneralResult< List<Role> >.Success(roles);

        }

        public async Task<GeneralResult<Role>> UpdateARole(byte Id, String newRoleName) 
        {
            var foundRole = await roleRepo.GetById(Id);
            if (foundRole == null) return GeneralResult<Role>.Error(
                204, "Role not found with Id:" +Id);

            foundRole.Name = newRoleName;
            roleRepo.Update(foundRole);
            await roleRepo.SaveAsync();

            return GeneralResult<Role>.Success(foundRole);
        }

        public async Task<GeneralResult<Role>> RemoveARole(byte roleId) 
        {
            var foundRole = await roleRepo.GetById(roleId);
            if (foundRole == null) return GeneralResult<Role>.Error(
                204, "Role not found with Id:" + roleId); ;

            roleRepo.Delete(foundRole);
            await roleRepo.SaveAsync();
            
            return GeneralResult<Role>.Success(foundRole);
        }

    }
}
