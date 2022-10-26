using BookingSoccers.Repo.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.IRepository.UserInfo
{
    public interface IUserRepo : IBaseRepository<User>
    {
          Task<User> GetByUserName(string UserName);
    }
}
