using BookingSoccers.Repo.IRepository.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.UserInfo;
using Microsoft.EntityFrameworkCore;

namespace BookingSoccers.Repo.Repository.UserInfo
{
    public class UserRepo : BaseRepository<User> ,IUserRepo
    {
        BookingSoccersContext bookingSoccersContext;
        public UserRepo(BookingSoccersContext _bookingSoccersContext) : base(_bookingSoccersContext)
        {
            bookingSoccersContext = _bookingSoccersContext;
        }

    }
}
