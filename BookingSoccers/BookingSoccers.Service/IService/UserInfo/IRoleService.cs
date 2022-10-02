using BookingSoccers.Repo.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.UserInfo
{
    public interface IRoleService
    {
        Task<Role> AddANewRole(string RoleName);

        Task<Role> RetrieveARoleById(int roleId);

        Task<List<Role>> RetrieveAllRoles();

        Task<Role> UpdateARole(int Id, string roleName);

        Task<Role> RemoveARole(int RoleId);

    }
}
