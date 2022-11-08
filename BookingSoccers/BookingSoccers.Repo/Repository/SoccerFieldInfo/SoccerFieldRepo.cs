using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.BookingInfo;
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
    public class SoccerFieldRepo : BaseRepository<SoccerField>, ISoccerFieldRepo
    {
        BookingSoccersContext bookingSoccersContext;
        public SoccerFieldRepo(BookingSoccersContext _bookingSoccersContext) : base(_bookingSoccersContext)
        {
            bookingSoccersContext = _bookingSoccersContext;
        }

        public async Task<SoccerField> GetFieldBookingScheduleOfADateByFieldId
            (int FieldId, DateTime date)
        {
            var returnedSoccerField = 
                await Get()
                .Include(x => x.Bookings).ThenInclude(y => y.Customer)
                .Include(x => x.Zones)
                .Where(x => x.Id == FieldId)
                .FirstOrDefaultAsync();

            if (returnedSoccerField != null)
            {
                var filteredBookings = returnedSoccerField.Bookings
                    .Where(x => x.StartTime.Date == date.Date).ToList();

                returnedSoccerField.Bookings = filteredBookings;
            }

            return returnedSoccerField;
        }

        public async Task<SoccerField> GetFieldByFieldName(string FieldName)
        {
            var Field = await Get()
                .Where(x => x.FieldName == FieldName)
                .FirstOrDefaultAsync();

            return Field;
        }

        public async Task<SoccerField> GetFieldDetails(int FieldId)
        {
            var FieldDetails = await Get()
                .Include(x => x.ImageList)
                .Include(x => x.PriceMenus)
                .ThenInclude(x => x.PriceItems)
                .Include(x => x.Zones)
                .Include(x => x.user)
                .Where(x => x.Id == FieldId)
                .FirstOrDefaultAsync();

            return FieldDetails;
        }

        public async Task<List<SoccerField>> GetFieldsForManagerByManagerId(int ManagerId)
        {
            var SoccerFieldList =
                await Get()
                .Include(x => x.ImageList)
                .Include(x => x.user)
                .Where(x => x.ManagerId == ManagerId)
                .ToListAsync();
        
            return SoccerFieldList;
        }

        public async Task<List<SoccerField>> 
            GetPaginationFieldList(int PageNum, int ManagerId)
        {
            var ReturnedFieldList = await Get()
                .Where(x => x.ManagerId == ManagerId)
                .OrderBy(x => x.Id)
                .Skip((PageNum - 1) * 20)
                .Take(20)
                .ToListAsync();

            return ReturnedFieldList;
        }

        public async Task<SoccerField> GetSoccerFieldByFieldId(int Id)
        {
            var returnedSoccerField =
                await Get()
                .Include(x => x.ImageList)
                .Include(x => x.PriceMenus)
                .ThenInclude(y => y.PriceItems)
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();

            return returnedSoccerField;
        }
    }
}
