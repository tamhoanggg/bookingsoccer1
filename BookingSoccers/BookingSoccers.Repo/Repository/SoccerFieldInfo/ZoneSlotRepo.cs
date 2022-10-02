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
    public class ZoneSlotRepo : BaseRepository<ZoneSlot>, IZoneSlotRepo
    {
        BookingSoccersContext bookingSoccersContext;
        public ZoneSlotRepo(BookingSoccersContext _bookingSoccersContext) : base(_bookingSoccersContext)
        {
            bookingSoccersContext = _bookingSoccersContext;
        }

    }
}
