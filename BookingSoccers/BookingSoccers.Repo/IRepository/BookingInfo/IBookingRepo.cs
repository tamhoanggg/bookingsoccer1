using BookingSoccers.Repo.Entities.BookingInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.IRepository.BookingInfo
{
    public interface IBookingRepo : IBaseRepository<Booking>
    {
        Task<Booking> GetBookingDetailsById(int id);

        Task<List<Booking>> GetSummaryBookingListByUserId(int UserId);

        Task<Booking> CheckBookingDuplicate(int ZoneId, DateTime Start, DateTime End);

        Task<Booking> GetPaymentsAndBookingById(int Id);

        Task<Booking> GetBookingInfoForCheckOut(int BookingId);

        Task<List<Booking>> GetBookingsByFieldId(int FieldId);

        Task<List<Booking>> GetBookingsDistinctByCustomerId(int FieldId);

        Task<List<Booking>> GetBookingsForReport
            (int FieldId, DateTime StartDate, DateTime EndDate);

        Task<List<Booking>> GetBookingsInLastMonth (int FieldId);

    }
}
