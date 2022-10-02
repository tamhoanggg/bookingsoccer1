using BookingSoccers.Repo.Entities.BookingInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.BookingInfo
{
    public interface IBookingService
    {
        Task<Booking> AddANewBooking(Booking bookinginfo);

        Task<Booking> RetrieveABookingById(int BookingId);

        Task<List<Booking>> RetrieveAllBooking();

        Task<Booking> UpdateABooking(int Id, Booking newBookingInfo);

        Task<Booking> RemoveABooking(int BookingId);
    }
}
