using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
using BookingSoccers.Service.IService.BookingInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO.Booking;
using BookingSoccers.Service.Models.DTO.Payment;
using BookingSoccers.Service.Models.Payload.Booking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Service.BookingInfo
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepo bookingRepo;
        private readonly IMapper mapper;

        public BookingService(IBookingRepo bookingRepo, IMapper mapper)
        {
            this.bookingRepo = bookingRepo;
            this.mapper = mapper;
        }

        public async Task<GeneralResult<Booking>> AddANewBooking
            (BookingCreatePayload bookinginfo)
        {
            var newBooking = mapper.Map<Booking>(bookinginfo);

            bookingRepo.Create(newBooking);
            await bookingRepo.SaveAsync();

            return GeneralResult<Booking>.Success(newBooking);
        }

        public async Task<GeneralResult<BookingView>> GetBookingAndPaymentsByUserId
            (int UserId)
        {
            var retrievedBookingPayments = 
                await bookingRepo.GetPaymentsAndBookingByUserId(UserId);

            if (retrievedBookingPayments == null) return GeneralResult<BookingView>.Error(
                404, "Booking not found with User Id:" + UserId);

            var BookingResult = mapper.Map<BookingView>(retrievedBookingPayments);

            List<PaymentView> paymentList = new List<PaymentView>();

            foreach (Payment payment in retrievedBookingPayments.payments)
            {
                var pay = mapper.Map<PaymentView>(payment);
                paymentList.Add(pay);
            }

            BookingResult.paymentsList = paymentList;

            return GeneralResult<BookingView>.Success(BookingResult);
        }

        public async Task<GeneralResult<Booking>> RemoveABooking(int BookingId)
        {
            var foundBooking = await bookingRepo.GetById(BookingId);

            if (foundBooking == null) return GeneralResult<Booking>.Error(
                404, "Booking not found with Id:" + BookingId);

            bookingRepo.Delete(foundBooking);
            await bookingRepo.SaveAsync();

            return GeneralResult<Booking>.Success(foundBooking);
        }

        public async Task<GeneralResult<Booking>> RetrieveABookingById(int BookingId)
        {
            var foundBooking = await bookingRepo.GetBookingDetailsById(BookingId);

            if (foundBooking == null) return GeneralResult<Booking>.Error(
                404, "Booking not found with Id:" + BookingId);

            return GeneralResult<Booking>.Success(foundBooking);
        }

        public async Task<GeneralResult<List<Booking>>> RetrieveAllBookings()
        {
            var BookingList = await bookingRepo.Get().ToListAsync();

            if (BookingList.Count == 0) return GeneralResult<List<Booking>>.Error(
                404, "No bookings found");

            return GeneralResult<List<Booking>>.Success(BookingList);
        }

        public async Task<GeneralResult<Booking>> UpdateABooking(int Id,
            BookingUpdatePayload newBookingInfo)
        {
            var toUpdateBooking = await bookingRepo.GetById(Id);

            if (toUpdateBooking == null) return GeneralResult<Booking>.Error(
                404, "No booking found with Id:" + Id);

            mapper.Map(newBookingInfo ,toUpdateBooking);
         
            bookingRepo.Update(toUpdateBooking);
            await bookingRepo.SaveAsync();

            return GeneralResult<Booking>.Success(toUpdateBooking);
        }

        public async Task<GeneralResult<Booking>> UpdateABookingZoneId(int Id, int ZoneId)
        {
            var toUpdateBookingZoneId = await bookingRepo.GetById(Id);

            if (toUpdateBookingZoneId == null) return GeneralResult<Booking>.Error(
                404, "No booking found with Id:" + Id);

            toUpdateBookingZoneId.ZoneId = ZoneId;

            bookingRepo.Update(toUpdateBookingZoneId);
            await bookingRepo.SaveAsync();

            return GeneralResult<Booking>.Success(toUpdateBookingZoneId);
        }

        public async Task<GeneralResult<BookingView>> UpdateBookingStatusForUser(int Id,
            StatusEnum newStatus)
        {
            var toUpdateBookingStatus = await bookingRepo.GetById(Id);

            if (toUpdateBookingStatus == null) return GeneralResult<BookingView>.Error(
                404, "No booking found with Id:" + Id);

            toUpdateBookingStatus.Status = newStatus;

            bookingRepo.Update(toUpdateBookingStatus);
            await bookingRepo.SaveAsync();

            var updatedBooking = mapper.Map<BookingView>(toUpdateBookingStatus);

            return GeneralResult<BookingView>.Success(updatedBooking);
        }
    }
}
