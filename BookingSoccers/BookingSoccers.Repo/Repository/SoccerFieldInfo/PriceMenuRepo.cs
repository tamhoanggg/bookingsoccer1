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
            PriceMenu? returnedPriceMenu = new();

                returnedPriceMenu = await Get()
                .Include(x => x.PriceItems)
                .Where(x => x.FieldId == FieldId && x.ZoneTypeId == ZoneTypeId && 
                x.DayType == DayTypeEnum.Holidays &&
                x.StartDate.Date <= date.Date && date.Date <= x.EndDate.Date )
                .FirstOrDefaultAsync();

            if (returnedPriceMenu != null) return returnedPriceMenu;

            var dayOfWeek = ((int)date.DayOfWeek);

            if(dayOfWeek <= 5) 
            {
                returnedPriceMenu = await Get()
                .Include(x => x.PriceItems)
                .Where(x => x.FieldId == FieldId && x.ZoneTypeId == ZoneTypeId &&
                x.DayType == DayTypeEnum.Weekdays &&
                x.StartDate.Date <= date.Date && date.Date <= x.EndDate.Date)
                .FirstOrDefaultAsync();

                return returnedPriceMenu;
            }

            returnedPriceMenu = await Get()
                .Include(x => x.PriceItems)
                .Where(x => x.FieldId == FieldId && x.ZoneTypeId == ZoneTypeId &&
                x.DayType == DayTypeEnum.Weekends &&
                x.StartDate.Date <= date.Date && date.Date <= x.EndDate.Date)
                .FirstOrDefaultAsync();

            return returnedPriceMenu;
        }

        public async Task<PriceMenu> GetFieldOpeningHour(int MenuId)
        {
            var ReturnedField = await Get()
                .Include(x => x.Field)
                .Where(x => x.Id == MenuId)
                .FirstOrDefaultAsync();

            return ReturnedField;
        }

        public async Task<List<PriceMenu>> GetPriceMenusForAField(int FieldId)
        {
            var PriceMenuList =
                await Get()
                .Include(x => x.PriceItems)
                .Include(x=> x.TypeOfZone)
                .Where(x => x.FieldId == FieldId)
                .ToListAsync();

            return PriceMenuList;
        }
    }
}
