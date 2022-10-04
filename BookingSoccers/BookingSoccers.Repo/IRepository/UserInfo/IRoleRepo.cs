using BookingSoccers.Repo.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.IRepository.UserInfo
{
    public interface IRoleRepo : IBaseRepository<Role>
    {
        Task<Role> GetRoleByName(String RoleName);   
    }
}
