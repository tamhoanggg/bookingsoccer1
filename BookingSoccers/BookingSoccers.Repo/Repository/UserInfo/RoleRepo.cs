using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.Repository.UserInfo
{
    public class RoleRepo : BaseRepository<Role>, IRoleRepo
    {
        BookingSoccersContext bookingSoccersContext;
        public RoleRepo(BookingSoccersContext _bookingSoccersContext) : base(_bookingSoccersContext)
        {
            bookingSoccersContext = _bookingSoccersContext;
        }

        public async Task<Role> GetRoleByName(string RoleName)
        {
            //if (bookingSoccersContext.Roles == null)
            //    return null;

            var returnedRole = await Get()
                .Where(x => x.Name == RoleName)
                .FirstOrDefaultAsync();

            return returnedRole;
        }

        public async Task<Role> GetUsersByRoleId(byte RoleId)
        {
            var UsersForRole = await Get()
                .Include(x => x.Users)
                .Where(y => y.Id == RoleId)
                .FirstOrDefaultAsync();

            return UsersForRole;
        }
    }
}
