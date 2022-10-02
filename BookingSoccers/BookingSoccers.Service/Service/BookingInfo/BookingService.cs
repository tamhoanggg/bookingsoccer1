using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
using BookingSoccers.Service.IService.BookingInfo;
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

        public BookingService(IBookingRepo bookingRepo)
        {
            this.bookingRepo = bookingRepo;
        }

        public async Task<Booking> AddANewBooking(Booking bookinginfo)
        {

            bookingRepo.Create(bookinginfo);
            await bookingRepo.SaveAsync();
            var createdBooking = await bookingRepo.GetById(bookinginfo.ZoneId);
            return createdBooking;
        }

        public async Task<Booking> RemoveABooking(int BookingId)
        {
            var foundBooking = await bookingRepo.GetById(BookingId);
            if (foundBooking == null) return null;
            bookingRepo.Delete(foundBooking);
            await bookingRepo.SaveAsync();
            return foundBooking;
        }

        public async Task<Booking> RetrieveABookingById(int BookingId)
        {
            var foundBooking = await bookingRepo.GetById(BookingId);
            if (foundBooking == null) return null;
            return foundBooking;
        }

        public async Task<List<Booking>> RetrieveAllBooking()
        {
            var BookingList = await bookingRepo.Get().ToListAsync();
            if (BookingList == null) return null;
            return BookingList;
        }

        public async Task<Booking> UpdateABooking(int Id, Booking newBookingInfo)
        {
            var toUpdateBooking = await RetrieveABookingById(Id);
            if (toUpdateBooking == null) return null;

            toUpdateBooking.Rating = newBookingInfo.Rating;
            toUpdateBooking.Status = newBookingInfo.Status;
            toUpdateBooking.StartTime = newBookingInfo.StartTime;
            toUpdateBooking.CreateTime = newBookingInfo.CreateTime;
            toUpdateBooking.Comment = newBookingInfo.Comment;
            toUpdateBooking.CustomerId = newBookingInfo.CustomerId;
            toUpdateBooking.EndTime = newBookingInfo.EndTime;
            toUpdateBooking.FieldId = newBookingInfo.FieldId;
            toUpdateBooking.TotalPrice = newBookingInfo.TotalPrice;
            toUpdateBooking.ZoneId = newBookingInfo.ZoneId;
            toUpdateBooking.ZoneTypeId = newBookingInfo.ZoneTypeId;

            bookingRepo.Update(toUpdateBooking);
            await bookingRepo.SaveAsync();

            return toUpdateBooking;
        }
    }
}
