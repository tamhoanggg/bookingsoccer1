using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Payment> GetPaymentDetail(int PaymentId)
        {
            var returnedPayment = await Get()
                .Include(x => x.ReceiverInfo)
                .Include(x => x.BookingInfo)
                .ThenInclude(x => x.Customer)
                .Where(x => x.Id == PaymentId)
                .FirstOrDefaultAsync();

            return returnedPayment;
        }
    }
}
