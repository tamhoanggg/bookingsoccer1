using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.DTO.Booking;
using BookingSoccers.Service.Models.DTO.Payment;
using BookingSoccers.Service.Models.DTO.User;
using BookingSoccers.Service.Models.Payload;
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

        Task<GeneralResult<Object>> GetABookingDetails(int BookingId);

        Task<GeneralResult<BookingView>> GetBookingAndPaymentsById(int Id);

        Task<GeneralResult<ObjectListPagingInfo>> RetrieveBookingsListForAdmin
            (PagingPayload pagingPayload, BookingPredicate filter);

        Task<GeneralResult<ObjectListPagingInfo>> RetrieveBookingsListForUser
            (PagingPayload pagingPayload, BookingPredicate filter);

        Task<GeneralResult<Booking>> UpdateABooking(int Id, BookingUpdatePayload newBookingInfo);

        Task<GeneralResult<Booking>> UpdateABookingZoneId(int Id, int ZoneId);

        Task<GeneralResult<Booking>> CheckOutABooking(int BookingId);

        Task<GeneralResult<BookingView>> UpdateBookingStatusForUser(int Id, StatusEnum newStatus);

        Task<GeneralResult<Booking>> RemoveABooking(int BookingId);
    }
}
