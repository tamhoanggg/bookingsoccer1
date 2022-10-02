using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
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

    }
}
