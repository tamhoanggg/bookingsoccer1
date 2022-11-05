using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
using BookingSoccers.Repo.Repository.UserInfo;
using BookingSoccers.Service.IService.UserInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.User;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace BookingSoccers.Service.UserInfo
{
    public class RoleService : IRoleService
    {
        private readonly IUserRepo userRepo;
        private readonly IRoleRepo roleRepo;
        //private readonly IMapper mapper;

        public RoleService(IRoleRepo roleRepo, IUserRepo userRepo)
        {
            this.roleRepo = roleRepo;
            this.userRepo = userRepo;
        }

        public async Task<GeneralResult<Role>> AddANewRole(string roleName)
        {
            //Check duplicate role name
            var existRole = await roleRepo.GetRoleByName(roleName);

            if (existRole != null) return GeneralResult<Role>
                    .Error(409 , "Role already exists");

            //Create a new instance of role and create it
            Role newRoleObj = new Role()
            {
                Name = roleName
            };
            roleRepo.Create(newRoleObj);
            await roleRepo.SaveAsync();

            return GeneralResult<Role>.Success(newRoleObj);
        }

        public async Task<GeneralResult<ObjectListPagingInfo>> RetrieveARoleDetails
            (PagingPayload pagingPayload, UserPredicate filter)
        {
            var newPred = PredicateBuilder.New<User>(true);

            //list of navi props to include in query
            string? includeList = null;

            //Predicates to add to query (given that any one of them isn't null) 
            if (filter.RoleId != null)
            {
                newPred = newPred.And(x => x.RoleId == filter.RoleId);
            }

            if (filter.GenderNum != null)
            {
                newPred = newPred.And(x => x.Gender == (GenderEnum)filter.GenderNum);
            }

            //Create a new expression instance
            Expression<Func<User, bool>>? pred = null;

            //If the Predicate Builder contains any predicate then
            //convert it to an expression
            if (!(newPred.Body.ToString().StartsWith("True") &&
                newPred.Body.ToString().EndsWith("True")))
            {
                pred = newPred;
            }

            //Get paged list of items with sort, filters, included navi props
            var returnedUserList = await userRepo.GetPaginationAsync
                (pagingPayload.PageNum, pagingPayload.OrderColumn,
                pagingPayload.IsAscending, includeList, pred);

            if (returnedUserList.Count() == 0) return
                    GeneralResult<ObjectListPagingInfo>
                    .Error(404, "No users found ");

            //Get total elements when running the query
            var TotalElement = await userRepo.GetPagingTotalElement(pred);

            //var UserList = mapper.Map<List<UserListInfo>>(returnedUserList);

            //Create new class to contain list result and paging info
            var FinalResult = new ObjectListPagingInfo();
            

            FinalResult.TotalElement = TotalElement;
            FinalResult.CurrentPage = pagingPayload.PageNum;

            //Calculate total pages based on total element
            var CheckRemain = TotalElement % 20;
            var SumPage = TotalElement / 20;
            if (CheckRemain > 0) FinalResult.TotalPage = SumPage + 1;
            else FinalResult.TotalPage = SumPage;

            //Get a role details
            var returnedRole = await roleRepo.GetById(filter.RoleId);

            if (returnedRole == null) return GeneralResult<ObjectListPagingInfo>.Error(
                404 , "Role not found with Id:"+ filter.RoleId);

            var UserListInfo = returnedUserList.Select(x => new 
            { 
                x.Id, x.UserName, Gender = x.Gender.ToString()
            }).ToList();

            var RoleInfo = new { returnedRole.Id, returnedRole.Name, UserListInfo};
            FinalResult.ObjectList = RoleInfo;

            return GeneralResult<ObjectListPagingInfo>.Success(FinalResult);
        }

        public async Task<GeneralResult < List<Role> > > RetrieveAllRoles() 
        { 
            //Return all roles
            var roles = await roleRepo.Get().ToListAsync();

            if (roles.Count ==0) return GeneralResult< List<Role> >.Error(
                404, "No role found");
            return GeneralResult< List<Role> >.Success(roles);

        }

        public async Task<GeneralResult<Role>> UpdateARole(byte Id, String newRoleName) 
        {
            //Get a specific role for update
            var foundRole = await roleRepo.GetById(Id);
            if (foundRole == null) return GeneralResult<Role>.Error(
                404, "Role not found with Id:" +Id);

            //Change the role name and update it
            foundRole.Name = newRoleName;
            roleRepo.Update(foundRole);
            await roleRepo.SaveAsync();

            return GeneralResult<Role>.Success(foundRole);
        }

        public async Task<GeneralResult<Role>> RemoveARole(byte roleId) 
        {
            //Get a specific role and remove it
            var foundRole = await roleRepo.GetById(roleId);
            if (foundRole == null) return GeneralResult<Role>.Error(
                404, "Role not found with Id:" + roleId); ;

            roleRepo.Delete(foundRole);
            await roleRepo.SaveAsync();
            
            return GeneralResult<Role>.Success(foundRole);
        }

        public async Task<GeneralResult<Role>> GetUsersOfARole(byte RoleId)
        {
            //Get users of a role by role Id
            var finalresult = await roleRepo.GetUsersByRoleId(RoleId);

            if(finalresult.Users == null) return GeneralResult<Role>.Error(
                404, "No users associated with role Id:" + RoleId);

            return GeneralResult<Role>.Success(finalresult);
        }
    }
}
