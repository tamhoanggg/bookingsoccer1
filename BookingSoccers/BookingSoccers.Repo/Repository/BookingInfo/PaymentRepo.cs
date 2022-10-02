using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.Repository.BookingInfo
{
    public class PaymentRepo : BaseRepository<Payment>, IPaymentRepo
    {
        BookingSoccersContext bookingSoccersContext;
        public PaymentRepo(BookingSoccersContext _bookingSoccersContext) : base(_bookingSoccersContext)
        {
            bookingSoccersContext = _bookingSoccersContext;
        }

    }
}
