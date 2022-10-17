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

        Task<Booking> CheckBookingDuplicate(DateTime Start, DateTime End);

        Task<Booking> GetPaymentsAndBookingByUserId(int UserId);

        Task<List<Booking>> GetBookingsByFieldId(int FieldId);

        Task<List<Booking>> GetBookingsDistinctByFieldId(int FieldId);

        Task<List<Booking>> GetBookingsForReport
            (int FieldId, DateTime StartDate, DateTime EndDate);

        Task<List<Booking>> GetBookingsInLastMonth (int FieldId);

    }
}
