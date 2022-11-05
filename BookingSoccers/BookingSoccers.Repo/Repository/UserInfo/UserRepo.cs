using BookingSoccers.Repo.IRepository.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.UserInfo;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LinqKit;
using LinqKit.Core;
using System.Reflection;

namespace BookingSoccers.Repo.Repository.UserInfo
{
    public class UserRepo : BaseRepository<User> ,IUserRepo
    {
        BookingSoccersContext bookingSoccersContext;

        public UserRepo(BookingSoccersContext _bookingSoccersContext) : base(_bookingSoccersContext)
        {
            bookingSoccersContext = _bookingSoccersContext;
        }

        public async Task<User> GetByUserName(string UserName)
        {
            var returnedUser = await Get()
                .Where(x => x.UserName == UserName)
                .FirstOrDefaultAsync();
            return returnedUser;
        }

        public async Task<User> GetUserDetails(int PageNum, int UserId)
        {
            var returnedUserDetails = await Get()
                .Include(x => x.SoccerFields)
                .Include(x => x.Bookings)
                .ThenInclude(x => x.payments)
                .Where(x => x.Id == UserId)
                .Skip((PageNum -1) * 20)
                .Take(20)
                .FirstOrDefaultAsync();

            return returnedUserDetails;
        }
    }
}
