using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
using BookingSoccers.Service.IService.UserInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Service.UserInfo
{
    public class UserService : IUserService
    {
        private readonly IUserRepo userRepo;
        private readonly BookingSoccersContext bookingSoccersContext;

        public UserService(IUserRepo userRepo, 
            BookingSoccersContext bookingSoccersContext)
        {
            this.userRepo = userRepo;
            this.bookingSoccersContext = bookingSoccersContext;
        }

        public async Task<User> AddANewUser(User userinfo)
        {
            if (await userRepo.GetById(userinfo.UserName) != null ||
                await userRepo.GetById(userinfo.Email) != null ||
                await userRepo.GetById(userinfo.PhoneNumber) != null) return null;
                userRepo.Create(userinfo);
            await userRepo.SaveAsync();
            return await userRepo.GetById(userinfo.UserName);
        }

        public async Task<User> GetByEmail(string email)
        {
            var User = await userRepo.GetById(email);
            if (User == null) return null;
            return User;
        }

        public async Task<User> RemoveAUser(int UserId)
        {
            var returnedUser = await userRepo.GetById(UserId);
            if (returnedUser == null) return null;
            userRepo.Delete(returnedUser);
            await userRepo.SaveAsync();
            return returnedUser;
        }

        public async Task<List<User>> RetrieveAllUsers()
        {
            var returnedUserList = await userRepo.Get().ToListAsync();
            if (returnedUserList == null) return null;
            return returnedUserList;
        }

        public async Task<User> RetrieveAUserById(int UserId)
        {
            var userById = await userRepo.GetById(UserId);
            if (userById == null) return null;
            return userById;
        }

        public async Task<User> UpdateAUser(int Id, User newUserInfo)
        {
            var toUpdateUser = await userRepo.GetById(Id);
            if (toUpdateUser == null) return null;

            toUpdateUser.UserName = newUserInfo.UserName;
            toUpdateUser.FirstName = newUserInfo.FirstName;
            toUpdateUser.LastName = newUserInfo.LastName;
            toUpdateUser.PhoneNumber = newUserInfo.PhoneNumber;
            toUpdateUser.Gender = newUserInfo.Gender;
            toUpdateUser.Email = newUserInfo.Email;

            userRepo.Update(toUpdateUser);
            await userRepo.SaveAsync();

            return toUpdateUser;
        }
    }
}
