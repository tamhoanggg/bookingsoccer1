using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.Repository.BookingInfo
{
    public class BookingRepo : BaseRepository<Booking>, IBookingRepo
    {
        BookingSoccersContext bookingSoccersContext;
        public BookingRepo(BookingSoccersContext _bookingSoccersContext) : base(_bookingSoccersContext)
        {
            bookingSoccersContext = _bookingSoccersContext;
        }

        public async Task<Booking> CheckBookingDuplicate(int ZoneId, DateTime Start, DateTime End)
        {
            var BookingResult = await Get()
                .Where(x => x.ZoneId == ZoneId && (Start < x.StartTime && End > x.EndTime) ||
                (x.StartTime == Start && x.EndTime == End) ||
                (x.StartTime <= Start && End < x.EndTime) || 
                (x.StartTime <= End && Start < x.EndTime))
                .FirstOrDefaultAsync();

            return BookingResult;
        }

        public async Task<Booking> GetBookingDetailsById(int id)
        {
            var bookingDetails = await Get()
                .Include(x => x.Customer)
                .Include(x => x.payments)
                .ThenInclude(x => x.ReceiverInfo)
                .Include(x => x.ZoneInfo)
                .Include(x => x.FieldInfo)
                .Include(x => x.TypeZone)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return bookingDetails;
        }

        public async Task<Booking> GetBookingInfoForCheckOut(int BookingId)
        {
            var toUpdateBooking = await Get()
                .Include(x => x.payments)
                .Where(x => x.Id == BookingId)
                .FirstOrDefaultAsync();

            return toUpdateBooking;
        }

        public async Task<List<Booking>> GetBookingsByFieldId(int FieldId)
        {
            var BookingList = await Get()
                .Where(x => x.FieldId == FieldId)
                .ToListAsync();

            return BookingList;
        }

        public async Task<List<Booking>> GetBookingsDistinctByCustomerId(int FieldId)
        {
            var BookingList = await Get()
                .Include(x => x.Customer)
                .Where(x => x.FieldId == FieldId)
                .DistinctBy(x => x.CustomerId)
                .ToListAsync();

            return BookingList;
        }

        public async Task<List<Booking>> GetBookingsForReport
            (int FieldId, DateTime StartDate, DateTime EndDate)
        {
            var BookingList = await Get()
                .Where(x => (x.FieldId == FieldId && x.Status == StatusEnum.CheckedOut) && 
                x.StartTime.Date <= StartDate.Date &&
                x.StartTime.Date >= EndDate.Date)
                .OrderBy(x => x.StartTime)
                .ToListAsync();

            return BookingList;
        }

        public async Task<List<Booking>> GetBookingsInLastMonth(int FieldId)
        {
            var Bookings = await Get()
                .Where(x => (x.FieldId == FieldId && x.Status == StatusEnum.CheckedOut) &&
                    x.StartTime <= DateTime.Now.Date && 
                    x.StartTime >= DateTime.Now.AddMonths(-1).Date)
                .ToListAsync();

            return Bookings;
        }

        public async Task<Booking> GetPaymentsAndBookingById(int id)
        {
            var BookingPayments = await Get()
                .Include(x => x.payments)
                .ThenInclude(y => y.ReceiverInfo)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            //

            //var final = Bookinglist
            //    .Select(x => new
            //    {
            //        x.StartTime.Month,
            //        x.StartTime.Year,
            //        x.TotalPrice,
            //    }
            //    ).GroupBy(x => new 
            //    {
            //        x.Month,
            //        x.Year
            //    }, (key, group) => new 
            //    {
            //        year = key.Year,
            //        month = key.Month,
            //        MonthTotal = group.Sum(y => y.TotalPrice)
            //    }
            //    ).ToList();
          
            return BookingPayments;
        }

        public async Task<List<Booking>> 
            GetBookingsPaginationByUserId(int PageNum, int UserId)
        {
            var returnedList = await Get()
                .Include(x => x.payments)
                .Where(x => x.CustomerId == UserId)
                .OrderBy(x => x.Id)
                .Skip((PageNum - 1) * 20)
                .Take(20)
                .ToListAsync();

            return returnedList;
        }

        public async Task<List<Booking>> GetSummaryBookingListByUserId(int UserId)
        {
            var bookingList = await Get()
                .Include(x => x.ZoneInfo)
                .Include(x => x.TypeZone)
                .Include(x => x.FieldInfo)
                .Where(x => x.CustomerId == UserId)
                .ToListAsync();

            return bookingList;
        }
    }
}
