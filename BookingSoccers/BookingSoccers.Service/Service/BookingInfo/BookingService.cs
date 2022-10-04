using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
using BookingSoccers.Service.IService.BookingInfo;
using BookingSoccers.Service.Models.Common;
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

        public async Task<GeneralResult<Booking>> AddANewBooking(BookingCreatePayload bookinginfo)
        {
            var newBooking = mapper.Map<Booking>(bookinginfo);

            bookingRepo.Create(newBooking);
            await bookingRepo.SaveAsync();

            return GeneralResult<Booking>.Success(newBooking);
        }

        public async Task<GeneralResult<Booking>> RemoveABooking(int BookingId)
        {
            var foundBooking = await bookingRepo.GetById(BookingId);

            if (foundBooking == null) return GeneralResult<Booking>.Error(
                204, "Booking not found with Id:" + BookingId);

            bookingRepo.Delete(foundBooking);
            await bookingRepo.SaveAsync();

            return GeneralResult<Booking>.Success(foundBooking);
        }

        public async Task<GeneralResult<Booking>> RetrieveABookingById(int BookingId)
        {
            var foundBooking = await bookingRepo.GetById(BookingId);

            if (foundBooking == null) return GeneralResult<Booking>.Error(
                204, "Booking not found with Id:" + BookingId);

            return GeneralResult<Booking>.Success(foundBooking);
        }

        public async Task<GeneralResult<List<Booking>>> RetrieveAllBookings()
        {
            var BookingList = await bookingRepo.Get().ToListAsync();

            if (BookingList == null) return GeneralResult<List<Booking>>.Error(
                204, "No bookings found");

            return GeneralResult<List<Booking>>.Success(BookingList);
        }

        public async Task<GeneralResult<Booking>> UpdateABooking(int Id, BookingUpdatePayload newBookingInfo)
        {
            var toUpdateBooking = await bookingRepo.GetById(Id);

            if (toUpdateBooking == null) return GeneralResult<Booking>.Error(
                204, "No booking found with Id:" + Id);

            mapper.Map(newBookingInfo ,toUpdateBooking);
         
            bookingRepo.Update(toUpdateBooking);
            await bookingRepo.SaveAsync();

            return GeneralResult<Booking>.Success(toUpdateBooking);
        }
    }
}
