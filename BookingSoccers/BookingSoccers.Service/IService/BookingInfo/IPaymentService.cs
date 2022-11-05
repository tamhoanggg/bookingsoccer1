using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.Payload.Booking;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.BookingInfo
{
    public interface IPaymentService
    {
        Task<GeneralResult<Payment>> AddANewPayment(PaymentCreatePayload paymentinfo);

        Task<GeneralResult<Object>> GetAPaymentDetails(int PaymentId);

        Task<GeneralResult<ObjectListPagingInfo>> RetrievePaymentsList
            (PagingPayload pagingPayload, PaymentPredicate filter);

        Task<GeneralResult<Payment>> UpdateAPayment(int Id, PaymentUpdatePayload newPaymentInfo);

        Task<GeneralResult<Payment>> RemoveAPayment(int PaymentId);
    }
}
