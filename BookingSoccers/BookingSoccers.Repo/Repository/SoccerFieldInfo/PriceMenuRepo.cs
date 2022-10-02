using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.Repository.SoccerFieldInfo
{
    public class PriceMenuRepo : BaseRepository<PriceMenu>, IPriceMenuRepo
    {
        BookingSoccersContext bookingSoccersContext;
        public PriceMenuRepo(BookingSoccersContext _bookingSoccersContext) : base(_bookingSoccersContext)
        {
            bookingSoccersContext = _bookingSoccersContext;
        }

    }
}
