using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.UserInfo
{
    public interface IUserService
    {
        public Task< GeneralResult<User> > GetByEmail(string email);

        Task< GeneralResult<User> > AddANewUser(UserCreatePayload userinfo);

        Task< GeneralResult<User> > RetrieveAUserById(int UserId);

        Task< GeneralResult< List<User> > > RetrieveAllUsers();

        Task< GeneralResult<User> > UpdateAUser(int Id, UserUpdatePayload newUserInfo);

        Task< GeneralResult<User> > RemoveAUser(int UserId);
    }
}
