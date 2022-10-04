using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.BookingInfo
{
    public interface IBookingService
    {
        Task<GeneralResult<Booking>> AddANewBooking(BookingCreatePayload bookinginfo);

        Task<GeneralResult<Booking>> RetrieveABookingById(int BookingId);

        Task<GeneralResult<List<Booking>>> RetrieveAllBookings();

        Task<GeneralResult<Booking>> UpdateABooking(int Id, BookingUpdatePayload newBookingInfo);

        Task<GeneralResult<Booking>> RemoveABooking(int BookingId);
    }
}
