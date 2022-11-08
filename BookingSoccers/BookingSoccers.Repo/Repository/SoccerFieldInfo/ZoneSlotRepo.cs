using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using Microsoft.EntityFrameworkCore;
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

        public async Task<DateTime> getAZoneSlotByZoneId(int Id)
        {
           var zoneSlot = await  Get().Where(x => x.ZoneId == Id).MaxAsync(x => x.EndTime);

            return zoneSlot;
        }

        public async Task<ZoneSlot> getAZoneSlotDetails(int ZoneSlotId)
        {
            var ZoneSlotDetails = await Get()
                .Include(x => x.FieldZone)
                .ThenInclude(x => x.Field)
                .Include(x => x.FieldZone)
                .ThenInclude(x => x.ZoneCate)
                .Where(x => x.Id == ZoneSlotId)
                .FirstOrDefaultAsync();

            return ZoneSlotDetails;
        }

        public async Task<List<ZoneSlot>> getZoneSlots(int ZoneId, DateTime date)
        {

            var resultList = await Get()
                    .Where(x => x.ZoneId == ZoneId && x.StartTime.Date == date.Date)
                    .ToListAsync();

            return resultList;
        }

        public async Task<List<ZoneSlot>> getZoneSlotsByZoneId
            (int ZoneId, DateTime date)
        {
            var resultList = await Get()
                    .Where(x => x.ZoneId == ZoneId && x.StartTime.Date == date.Date 
                    && x.Status ==0)
                    .ToListAsync();

            return resultList;
        }

    }
}
