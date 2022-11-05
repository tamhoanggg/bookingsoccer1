using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.Payload.User;
using BookingSoccers.Service.Models.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.UserInfo
{
    public interface IRoleService
    {
        Task<GeneralResult<Role>> AddANewRole(string RoleName);

        Task<GeneralResult<ObjectListPagingInfo>> RetrieveARoleDetails
            (PagingPayload pagingPayload, UserPredicate filter);

        Task<GeneralResult< List<Role> > > RetrieveAllRoles();

        Task<GeneralResult<Role>> UpdateARole(byte Id, string roleName);

        Task<GeneralResult<Role>> RemoveARole(byte RoleId);

        Task<GeneralResult<Role>> GetUsersOfARole(byte RoleId);
    }
}
