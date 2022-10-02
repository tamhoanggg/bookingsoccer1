using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Service.IService.BookingInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Service.BookingInfo
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepo paymentRepo;

        public PaymentService(IPaymentRepo paymentRepo)
        {
            this.paymentRepo = paymentRepo;
        }

        public async Task<Payment> AddANewPayment(Payment paymentinfo)
        {
            paymentRepo.Create(paymentinfo);
            await paymentRepo.SaveAsync();
            return paymentinfo;
        }

        public async Task<Payment> RemoveAPayment(int PaymentId)
        {
            var searchPayment = await RetrieveAPaymentById(PaymentId);
            if (searchPayment == null) return null;
            paymentRepo.Delete(searchPayment);
            await paymentRepo.SaveAsync();
            return searchPayment;
        }

        public async Task<Payment> RetrieveAPaymentById(int PaymentId)
        {
            var returnedPayment = await paymentRepo.GetById(PaymentId);
            if (returnedPayment == null) return null;
            return returnedPayment;
        }

        public async Task<List<Payment>> RetrieveAllPayments()
        {
            var PaymentList = await paymentRepo.Get().ToListAsync();
            if (PaymentList == null) return null;
            return PaymentList;
        }

        public async Task<Payment> UpdateAPayment(int Id, Payment newPaymentInfo)
        {
            var toUpdatePayment = await RetrieveAPaymentById(Id);
            if (toUpdatePayment == null) return null;

            toUpdatePayment.Type = newPaymentInfo.Type;
            toUpdatePayment.BookingId = newPaymentInfo.BookingId;
            toUpdatePayment.Amount = newPaymentInfo.Amount;
            toUpdatePayment.ReceiverId = newPaymentInfo.ReceiverId;
            toUpdatePayment.Time = newPaymentInfo.Time;
            
            paymentRepo.Update(toUpdatePayment);
            await paymentRepo.SaveAsync();

            return toUpdatePayment;
        }
    }
}
