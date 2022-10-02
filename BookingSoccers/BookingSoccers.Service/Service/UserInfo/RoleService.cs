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

        public async Task<Role> AddANewRole(string roleName)
        {
            var existRole = roleRepo.GetById(roleName);
            if (existRole != null) return null;

            Role newRoleObj = new Role()
            {
                Name = roleName
            };
            roleRepo.Create(newRoleObj);
            await roleRepo.SaveAsync();
            return newRoleObj;
        }

        public async Task<Role> RetrieveARoleById(int roleId)
        {
            Role returnedRole = await roleRepo.GetById(roleId);
            if (returnedRole == null) return null;
            return returnedRole;
        }

        public async Task<List<Role>> RetrieveAllRoles() 
        { 
            List<Role> roles = await roleRepo.Get().ToListAsync();
            return roles;

        }

        public async Task<Role> UpdateARole(int Id, String newRoleName) 
        {
            Role foundRole = await roleRepo.GetById(Id);
            if (foundRole == null) return null;

            foundRole.Name = newRoleName;
            roleRepo.Update(foundRole);
            await roleRepo.SaveAsync();

            return foundRole;
        }

        public async Task<Role> RemoveARole(int roleId) 
        {
            Role foundRole = await roleRepo.GetById(roleId);
            if (foundRole == null) return null;

            roleRepo.Delete(foundRole);
            await roleRepo.SaveAsync();
            
            return foundRole;
        }

    }
}
