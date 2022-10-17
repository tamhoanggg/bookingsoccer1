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
    public class PriceMenuRepo : BaseRepository<PriceMenu>, IPriceMenuRepo
    {
        BookingSoccersContext bookingSoccersContext;
        public PriceMenuRepo(BookingSoccersContext _bookingSoccersContext) : base(_bookingSoccersContext)
        {
            bookingSoccersContext = _bookingSoccersContext;
        }

        public async Task<PriceMenu> GetAPriceMenu(int FieldId, DateTime date, byte ZoneTypeId)
        {
            var HolidayPriceMenu = await Get()
                .Include(x => x.PriceItems)
                .Where(x => x.FieldId == FieldId && x.ZoneTypeId == ZoneTypeId && 
                x.DayType == DayTypeEnum.Holidays &&
                x.StartDate.Date <= date.Date && date.Date <= x.EndDate.Date )
                .FirstOrDefaultAsync();

            if (HolidayPriceMenu != null) return HolidayPriceMenu;

            var PriceMenu = await Get()
                .Include(x => x.PriceItems)
                .Where(x => x.FieldId == FieldId && x.ZoneTypeId == ZoneTypeId &&
                x.StartDate.Date <= date.Date && date.Date <= x.EndDate.Date)
                .FirstOrDefaultAsync();

            return PriceMenu;
        }

        public async Task<List<PriceMenu>> GetPriceMenusForAField(int FieldId)
        {
            var PriceMenuList =
                await Get()
                .Include(x => x.PriceItems)
                .Where(x => x.FieldId == FieldId)
                .ToListAsync();

            return PriceMenuList;
        }
    }
}
