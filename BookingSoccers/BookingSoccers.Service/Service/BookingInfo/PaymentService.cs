using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.Repository.UserInfo;
using BookingSoccers.Service.IService.BookingInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.DTO.User;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.Payment;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            //Mapping new payment info to new Payment instance
            //and create a new payment
            var payment = mapper.Map<Payment>(paymentinfo);

            paymentRepo.Create(payment);
            await paymentRepo.SaveAsync();

            return GeneralResult<Payment>.Success(payment);
        }

        public async Task<GeneralResult<Payment>> RemoveAPayment(int PaymentId)
        {
            //Get requested payment for removing then remove it
            var searchPayment = await paymentRepo.GetById(PaymentId);

            if (searchPayment == null) return GeneralResult<Payment>.Error(
                404, "Payment not found with Id:" + PaymentId);

            paymentRepo.Delete(searchPayment);
            await paymentRepo.SaveAsync();

            return GeneralResult<Payment>.Success(searchPayment);
        }

        public async Task<GeneralResult<Object>> GetAPaymentDetails(int PaymentId)
        {
            //Get requested payment details and return it
            var returnedPaymentDetails = await paymentRepo
                .GetPaymentDetail(PaymentId);

            if (returnedPaymentDetails == null) return GeneralResult<Object>.Error(
                404, "Payment not found with Id:" + PaymentId);

            var FinalResult = new
            {
                returnedPaymentDetails.Id,
                ReceiverInfo = new 
                {
                    returnedPaymentDetails.ReceiverId,
                    returnedPaymentDetails.ReceiverInfo.UserName,
                    Gender = returnedPaymentDetails.ReceiverInfo.Gender.ToString(),
                    returnedPaymentDetails.ReceiverInfo.PhoneNumber,
                    returnedPaymentDetails.ReceiverInfo.Email
                },
                BookingInfo = new
                {
                    returnedPaymentDetails.BookingInfo.Id,
                    returnedPaymentDetails.BookingInfo.TotalPrice,
                    StartTime = returnedPaymentDetails.BookingInfo.StartTime.ToLocalTime(),
                    EndTime = returnedPaymentDetails.BookingInfo.EndTime.ToLocalTime(),
                    CreateTime = returnedPaymentDetails.BookingInfo.CreateTime.ToLocalTime(),
                    Status = returnedPaymentDetails.BookingInfo.Status.ToString(), 
                    returnedPaymentDetails.BookingInfo.Rating, 
                    returnedPaymentDetails.BookingInfo.Comment
                }

            };

            return GeneralResult<Object>.Success(FinalResult);
        }

        public async Task<GeneralResult<ObjectListPagingInfo>> RetrievePaymentsList
            (PagingPayload pagingPayload, PaymentPredicate filter)
        {
            //Create a new instance of predicate builder used to build query predicate
            var newPred = PredicateBuilder.New<Payment>(true);

            //list of navi props to include in query
            string? includeList = "ReceiverInfo,";

            //Predicates to add to query (given that any one of them isn't null) 
            if (filter.FromPrice != null || filter.ToPrice != null)
            {
                if (filter.FromPrice != null && filter.ToPrice != null)
                {
                    newPred = newPred.And(x => filter.FromPrice <= x.Amount);
                    newPred = newPred.And(x => filter.ToPrice >= x.Amount);
                }

                if (filter.FromPrice != null && filter.ToPrice == null)
                {
                    newPred = newPred.And(x => filter.FromPrice <= x.Amount);
                }

                if (filter.FromPrice == null && filter.ToPrice != null)
                {
                    newPred = newPred.And(x => filter.ToPrice >= x.Amount);
                }
            }

            if (filter.CreateTimeFrom != null || filter.CreateTimeTo != null)
            {
                DateTime? start = null;
                DateTime? end = null;

                if (filter.CreateTimeFrom != null)
                {
                    start = filter.CreateTimeFrom;

                    if (filter.CreateTimeFrom.Value.Kind == DateTimeKind.Local ||
                        filter.CreateTimeFrom.Value.Kind == DateTimeKind.Unspecified)
                    {
                        start.Value.ToUniversalTime();
                    }
                }

                if (filter.CreateTimeTo != null)
                {
                    end = filter.CreateTimeTo;

                    if (filter.CreateTimeTo.Value.Kind == DateTimeKind.Local ||
                        filter.CreateTimeTo.Value.Kind == DateTimeKind.Unspecified)
                    {
                        end.Value.ToUniversalTime();
                    }
                }

                if (filter.CreateTimeFrom != null && filter.CreateTimeTo != null)
                {
                    newPred = newPred.And(x => start <= x.Time);
                    newPred = newPred.And(x => end >= x.Time);
                }

                if (filter.CreateTimeFrom != null && filter.CreateTimeTo == null)
                {
                    newPred = newPred.And(x => start <= x.Time);
                }

                if (filter.CreateTimeFrom == null && filter.CreateTimeTo != null)
                {
                    newPred = newPred.And(x => end >= x.Time);
                }
            }

            if(filter.PaymentType != null) 
            {
               newPred = newPred.And(x => x.Type == (PaymentTypeEnum)filter.PaymentType);
            }

            //Create a new expression instance
            Expression<Func<Payment, bool>>? pred = null;

            //If the Predicate Builder contains any predicate then
            //convert it to an expression
            if (!(newPred.Body.ToString().StartsWith("True") &&
                newPred.Body.ToString().EndsWith("True")))
            {
                pred = newPred;
            }

            //Get paged list of items with sort, filters, included navi props
            var returnedPaymentList = await paymentRepo.GetPaginationAsync
                (pagingPayload.PageNum, pagingPayload.OrderColumn, 
                pagingPayload.IsAscending, includeList,
                 pred);

            if (returnedPaymentList.Count() == 0) return
                GeneralResult<ObjectListPagingInfo>
                    .Error(404, "No payments found ");

            //Get total elements when running the query
            var TotalElement = await paymentRepo.GetPagingTotalElement(pred);

            //Create new class to contain list result and paging info
            var FinalResult = new ObjectListPagingInfo();

            //Create new desired response 
            FinalResult.ObjectList = returnedPaymentList.Select(x => new 
            {
                x.Id, x.BookingId, ReceiverName = x.ReceiverInfo.UserName, 
                x.ReceiverId, x.Type, x.Amount, x.Time
            }).ToList();

            FinalResult.TotalElement = TotalElement;
            FinalResult.CurrentPage = pagingPayload.PageNum;

            //Calculate total pages based on total element
            var CheckRemain = TotalElement % 20;
            var SumPage = TotalElement / 20;
            if (CheckRemain > 0) FinalResult.TotalPage = SumPage + 1;
            else FinalResult.TotalPage = SumPage;

            return GeneralResult <ObjectListPagingInfo>.Success(FinalResult);
        }

        public async Task<GeneralResult<Payment>> UpdateAPayment(int Id, 
            PaymentUpdatePayload newPaymentInfo)
        {
            //Get a payment details for update based on Payment id
            var toUpdatePayment = await paymentRepo.GetById(Id);
            if (toUpdatePayment == null) return GeneralResult<Payment>.Error(
                404, "Payment not found with Id:" + Id);

            //Mapping new info to returned payment and update
            mapper.Map(newPaymentInfo, toUpdatePayment);

            paymentRepo.Update(toUpdatePayment);
            await paymentRepo.SaveAsync();

            return GeneralResult<Payment>.Success(toUpdatePayment);
        }
    }
}
