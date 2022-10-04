using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Service.IService.BookingInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.Payment;
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
        private readonly IMapper mapper;

        public PaymentService(IPaymentRepo paymentRepo, IMapper mapper)
        {
            this.paymentRepo = paymentRepo;
            this.mapper = mapper;
        }

        public async Task<GeneralResult<Payment>> AddANewPayment(PaymentCreatePayload paymentinfo)
        {
            var payment = mapper.Map<Payment>(paymentinfo);

            paymentRepo.Create(payment);
            await paymentRepo.SaveAsync();

            return GeneralResult<Payment>.Success(payment);
        }

        public async Task<GeneralResult<Payment>> RemoveAPayment(int PaymentId)
        {
            var searchPayment = await paymentRepo.GetById(PaymentId);

            if (searchPayment == null) return GeneralResult<Payment>.Error(
                204, "Payment not found with Id:" + PaymentId);

            paymentRepo.Delete(searchPayment);
            await paymentRepo.SaveAsync();

            return GeneralResult<Payment>.Success(searchPayment);
        }

        public async Task<GeneralResult<Payment>> RetrieveAPaymentById(int PaymentId)
        {
            var returnedPayment = await paymentRepo.GetById(PaymentId);

            if (returnedPayment == null) return GeneralResult<Payment>.Error(
                204, "Payment not found with Id:" + PaymentId);

            return GeneralResult<Payment>.Success(returnedPayment);
        }

        public async Task<GeneralResult<List<Payment>>> RetrieveAllPayments()
        {
            var PaymentList = await paymentRepo.Get().ToListAsync();

            if (PaymentList == null) return GeneralResult<List<Payment>>.Error(
                204, "No payment found ");

            return GeneralResult <List<Payment>>.Success(PaymentList);
        }

        public async Task<GeneralResult<Payment>> UpdateAPayment(int Id, 
            PaymentUpdatePayload newPaymentInfo)
        {
            var toUpdatePayment = await paymentRepo.GetById(Id);
            if (toUpdatePayment == null) return GeneralResult<Payment>.Error(
                204, "Payment not found with Id:" + Id);

            mapper.Map(newPaymentInfo, toUpdatePayment);

            paymentRepo.Update(toUpdatePayment);
            await paymentRepo.SaveAsync();

            return GeneralResult<Payment>.Success(toUpdatePayment);
        }
    }
}
