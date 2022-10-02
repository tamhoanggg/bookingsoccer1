using BookingSoccers.Repo.Entities.BookingInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.BookingInfo
{
    public interface IPaymentService
    {
        Task<Payment> AddANewPayment(Payment paymentinfo);

        Task<Payment> RetrieveAPaymentById(int PaymentId);

        Task<List<Payment>> RetrieveAllPayments();

        Task<Payment> UpdateAPayment(int Id, Payment newPaymentInfo);

        Task<Payment> RemoveAPayment(int PaymentId);
    }
}
