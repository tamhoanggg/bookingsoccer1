using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Service.Models.Common;
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

        Task<GeneralResult<Role>> RetrieveARoleById(byte roleId);

        Task<GeneralResult< List<Role> > > RetrieveAllRoles();

        Task<GeneralResult<Role>> UpdateARole(byte Id, string roleName);

        Task<GeneralResult<Role>> RemoveARole(byte RoleId);

    }
}
