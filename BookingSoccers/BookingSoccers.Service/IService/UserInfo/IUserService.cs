using BookingSoccers.Repo.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.UserInfo
{
    public interface IUserService
    {
        public Task<User> GetByEmail(string email);

        Task<User> AddANewUser(User userinfo);

        Task<User> RetrieveAUserById(int UserId);

        Task<List<User>> RetrieveAllUsers();

        Task<User> UpdateAUser(int Id, User newUserInfo);

        Task<User> RemoveAUser(int UserId);
    }
}
