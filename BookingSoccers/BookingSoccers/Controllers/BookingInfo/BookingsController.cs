using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Service.IService.BookingInfo;
using BookingSoccers.Service.IService.UserInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.Booking;
using BookingSoccers.Service.Models.Payload.Payment;
using BookingSoccers.Service.Models.Payload.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingSoccers.Controllers.BookingInfo
{
   [Route("api/v1/bookings")]
   [ApiController]
   [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly BookingSoccersContext bookingSoccersContext;
        private readonly IBookingService bookingService;
        private readonly IPaymentService paymentService;
        private readonly IMapper mapper;

        public BookingsController(BookingSoccersContext bookingSoccersContext,
            IBookingService bookingService, IMapper mapper, IPaymentService paymentService)
        {
            this.bookingService = bookingService;
            this.bookingSoccersContext = bookingSoccersContext;
            this.mapper = mapper;
            this.paymentService = paymentService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {

            var result = await bookingService.RetrieveAllBookings();

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneSpecificBooking(int id)
        {
            var retrievedBooking = await bookingService.RetrieveABookingById(id);

            if (retrievedBooking.IsSuccess)
                return Ok(retrievedBooking);

            Response.StatusCode = retrievedBooking.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedBooking);

            return StatusCode(retrievedBooking.StatusCode, response);
        }

        [Authorize(Roles = "User")]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetOneBookingsAndItsPayments(int id)
        {
            var retrievedBooking = await bookingService.GetBookingAndPaymentsById(id);

            if (retrievedBooking.IsSuccess)
                return Ok(retrievedBooking);

            Response.StatusCode = retrievedBooking.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedBooking);

            return StatusCode(retrievedBooking.StatusCode, response);
        }

        [Authorize(Roles = "User")]
        [HttpGet("user")]
        public async Task<IActionResult> GetBookingsOfAUser(int UserId)
        {
            var retrievedBookingList = await bookingService
                .GetBookingsOfAUser(UserId);

            if (retrievedBookingList.IsSuccess)
                return Ok(retrievedBookingList);

            Response.StatusCode = retrievedBookingList.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedBookingList);

            return StatusCode(retrievedBookingList.StatusCode, response);
        }

        [Authorize(Roles ="User,Admin")]
        [HttpPost]
        public async Task<IActionResult> AddANewBooking
            (BookingCreatePayload newBookingInfo)
        {
            var AddedBooking = await bookingService.AddANewBooking(newBookingInfo);

            if (AddedBooking.IsSuccess)
                return Ok(AddedBooking);

            Response.StatusCode = AddedBooking.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedBooking);

            return StatusCode(AddedBooking.StatusCode, response);
        }

        [Authorize(Roles ="User,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateABooking(int id,
            BookingUpdatePayload NewUserInfo)
        {


            var updatedBooking = await bookingService.UpdateABooking(id, NewUserInfo);

            if (updatedBooking.IsSuccess)
                return Ok(updatedBooking);

            Response.StatusCode = updatedBooking.StatusCode;

            var response = mapper.Map<ErrorResponse>(updatedBooking);

            return StatusCode(updatedBooking.StatusCode, response);
        }

        [Authorize(Roles = "User")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateABookingStatus(int id,
            StatusEnum newBookingStatus)
        {


            var updatedBookingStatus = await bookingService.UpdateBookingStatusForUser(id, newBookingStatus);

            if (updatedBookingStatus.IsSuccess)
                return Ok(updatedBookingStatus);

            Response.StatusCode = updatedBookingStatus.StatusCode;

            var response = mapper.Map<ErrorResponse>(updatedBookingStatus);

            return StatusCode(updatedBookingStatus.StatusCode, response);
        }

        [Authorize(Roles = "FieldManager")]
        [HttpPut("{id}/zone-id")]
        public async Task<IActionResult> UpdateBookingZoneId(int id, int ZoneId) 
        {
            var updatedBookingZoneId = 
                await bookingService.UpdateABookingZoneId(id, ZoneId);

            if (updatedBookingZoneId.IsSuccess)
                return Ok(updatedBookingZoneId);

            Response.StatusCode = updatedBookingZoneId.StatusCode;

            var response = mapper.Map<ErrorResponse>(updatedBookingZoneId);

            return StatusCode(updatedBookingZoneId.StatusCode, response);
        }

        [Authorize(Roles = "FieldManager")]
        [HttpPut("{id}/check-out")]
        public async Task<IActionResult> CheckOutABooking(int id)
        {
            var updatedBooking =
                await bookingService.CheckOutABooking(id);

            if (updatedBooking.IsSuccess)
                return Ok(updatedBooking);

            Response.StatusCode = updatedBooking.StatusCode;

            var response = mapper.Map<ErrorResponse>(updatedBooking);

            return StatusCode(updatedBooking.StatusCode, response);
        }

        [Authorize(Roles ="User,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteABooking(int id)
        {
            var deletedBooking = await bookingService.RemoveABooking(id);

            if (deletedBooking.IsSuccess)
                return Ok(deletedBooking);

            Response.StatusCode = deletedBooking.StatusCode;

            var response = mapper.Map<ErrorResponse>(deletedBooking);

            return StatusCode(deletedBooking.StatusCode, response);
        }
    }
}
