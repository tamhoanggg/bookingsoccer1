using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.DTO.User;
using BookingSoccers.Service.Models.Payload;
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
        Task< GeneralResult<User> > GetByEmail(string email);

        Task<GeneralResult<User>> GetAUserByUserName(string UserName);

        Task< GeneralResult<User> > AddANewUser(UserCreatePayload userinfo);

        Task< GeneralResult<ObjectListPagingInfo> > RetrieveAUserDetails
            (int PageNum, int UserId);

        Task<GeneralResult<BasicUserInfo>> RetrieveAUserForUpdate(string UserName);

        Task< GeneralResult<ObjectListPagingInfo>> RetrieveUsersList
            (PagingPayload pagingPayload, UserPredicate filter);

        Task< GeneralResult<User> > UpdateAUser(int Id, UserUpdatePayload newUserInfo);

        Task< GeneralResult<BasicUserInfo>> UpdateUserInfoForUser(int id,UserUpdatePayload newUserInfo);

        Task< GeneralResult<User> > RemoveAUser(int UserId);
    }
}
