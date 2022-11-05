using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
using BookingSoccers.Repo.Repository.UserInfo;
using BookingSoccers.Service.IService.BookingInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.DTO.Booking;
using BookingSoccers.Service.Models.DTO.Payment;
using BookingSoccers.Service.Models.DTO.User;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.Booking;
using Firebase.Auth;
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
    public class BookingService : IBookingService
    {
        private readonly IBookingRepo bookingRepo;
        private readonly IPaymentRepo paymentRepo;
        private readonly IMapper mapper;

        public BookingService(IBookingRepo bookingRepo, IPaymentRepo paymentRepo,
            IMapper mapper)
        {
            this.bookingRepo = bookingRepo;
            this.paymentRepo = paymentRepo;
            this.mapper = mapper;
        }

        public async Task<GeneralResult<Booking>> AddANewBooking
            (BookingCreatePayload bookinginfo)
        {
            //mapping info for creating a new booking
            var newBooking = mapper.Map<Booking>(bookinginfo);

            bookingRepo.Create(newBooking);
            await bookingRepo.SaveAsync();

            return GeneralResult<Booking>.Success(newBooking);
        }

        public async Task<GeneralResult<Booking>> CheckOutABooking(int BookingId)
        {
            //Get the booking with status = CheckedIn that needs to check out
            var returnedBooking = await bookingRepo.GetBookingInfoForCheckOut(BookingId);

            if (returnedBooking == null) return GeneralResult<Booking>.Error
                    (404, "No booking found to udpate");

            //Change status from CheckedIn => CheckedOut and update
            returnedBooking.Status = StatusEnum.CheckedOut;

            bookingRepo.Update(returnedBooking);
            await bookingRepo.SaveAsync();

            //Create a new postpay payment after checking out

            var newPayment = new Payment()
            {
                BookingId = BookingId,
                ReceiverId = returnedBooking.payments.First().ReceiverId,
                Type = PaymentTypeEnum.PostPay,
                Amount =
                returnedBooking.TotalPrice - returnedBooking.payments.First().Amount,
                Time = DateTime.UtcNow
            };

            paymentRepo.Create(newPayment);
            await paymentRepo.SaveAsync();

            return GeneralResult<Booking>.Success(returnedBooking);
        }

        public async Task<GeneralResult<BookingView>> GetBookingAndPaymentsById
            (int Id)
        {
            //Get a booking and its payments by BookingId
            var retrievedBookingPayments = 
                await bookingRepo.GetPaymentsAndBookingById(Id);

            if (retrievedBookingPayments == null) return GeneralResult<BookingView>.Error(
                404, "Booking not found with User Id:" + Id);

            //Mapping to Booking DTO
            var BookingResult = mapper.Map<BookingView>(retrievedBookingPayments);

            List<PaymentView> paymentList = new List<PaymentView>();

            //Mapping payments list to Payment DTO
            foreach (Payment payment in retrievedBookingPayments.payments)
            {
                var pay = mapper.Map<PaymentView>(payment);
                paymentList.Add(pay);
            }
            //And assign it to Booking DTO
            BookingResult.paymentsList = paymentList;

            return GeneralResult<BookingView>.Success(BookingResult);
        }

        public async Task<GeneralResult<Booking>> RemoveABooking(int BookingId)
        {
            //Get a booking that needs to be deleted
            var foundBooking = await bookingRepo.GetById(BookingId);

            if (foundBooking == null) return GeneralResult<Booking>.Error(
                404, "Booking not found with Id:" + BookingId);

            bookingRepo.Delete(foundBooking);
            await bookingRepo.SaveAsync();

            return GeneralResult<Booking>.Success(foundBooking);
        }

        public async Task<GeneralResult<Object>> GetABookingDetails(int BookingId)
        {
            //Get details of the requested Booking by Booking Id
            var foundBooking = await bookingRepo.GetBookingDetailsById(BookingId);

            if (foundBooking == null) return GeneralResult<Object>.Error(
                404, "Booking not found with Id:" + BookingId);

            var ReturnedResult = new 
            {
                foundBooking.Id, 
                UserInfo = new 
                { 
                    foundBooking.CustomerId, 
                    foundBooking.Customer.UserName, foundBooking.Customer.FirstName, 
                    foundBooking.Customer.LastName, 
                    Gender = foundBooking.Customer.Gender.ToString(), 
                    foundBooking.Customer.PhoneNumber, foundBooking.Customer.Email 
                },
                PaymentInfo = foundBooking.payments.Select(x => new 
                {
                    x.Id, x.ReceiverId, ReceiverName = x.ReceiverInfo.UserName, 
                    PaymentType = x.Type.ToString(), x.Amount, CreateTime = x.Time
                }).ToList(),
                FieldInfo = new
                {
                    foundBooking.FieldId, foundBooking.FieldInfo.ManagerId,
                    foundBooking.FieldInfo.FieldName, foundBooking.FieldInfo.OpenHour,
                    foundBooking.FieldInfo.CloseHour, foundBooking.FieldInfo.Address
                },
                ZoneInfo = new 
                {
                    foundBooking.ZoneId, foundBooking.ZoneInfo.Number
                },
                ZoneTypeInfo = new 
                { 
                    foundBooking.ZoneTypeId, foundBooking.TypeZone.Name 
                }, 
                foundBooking.TotalPrice, StartTime = foundBooking.StartTime.ToLocalTime(),
                EndTime = foundBooking.EndTime.ToLocalTime(), 
                CreateTime = foundBooking.CreateTime.ToLocalTime(), 
                Status = foundBooking.Status.ToString(), foundBooking.Rating, 
                foundBooking.Comment
            };

            return GeneralResult<Object>.Success(ReturnedResult);
        }

        public async Task<GeneralResult<ObjectListPagingInfo>> 
            RetrieveBookingsListForAdmin
            (PagingPayload pagingPayload, BookingPredicate filter)
        {
            //Create a new instance of predicate builder used to build query predicate
            var newPred = PredicateBuilder.New<Booking>(true);
            
            //list of navi props to include in query 
            string? includeList = "Customer,TypeZone,ZoneInfo,FieldInfo";

            //Predicates to add to query (given that any one of them isn't null) 
            if (filter.ZoneTypeId != null)
            {
                newPred = newPred.And(x => x.ZoneTypeId == filter.ZoneTypeId);
            }

            if (filter.FromPrice != null || filter.ToPrice != null)
            {
                if(filter.FromPrice != null && filter.ToPrice != null) 
                {
                    newPred = newPred.And(x => filter.FromPrice <= x.TotalPrice);
                    newPred = newPred.And(x => filter.ToPrice >= x.TotalPrice);
                }
                
                if (filter.FromPrice != null && filter.ToPrice == null)
                {
                    newPred = newPred.And(x => filter.FromPrice <= x.TotalPrice);
                }

                if (filter.FromPrice == null && filter.ToPrice != null)
                {
                    newPred = newPred.And(x => filter.ToPrice >= x.TotalPrice);
                }
            }

            if(filter.BookingDateFrom != null || filter.BookingDateTo != null) 
            {
                DateTime? start = null;
                DateTime? end = null;

                if(filter.BookingDateFrom != null) 
                {
                    start = filter.BookingDateFrom;

                    if (filter.BookingDateFrom.Value.Kind == DateTimeKind.Local ||
                        filter.BookingDateFrom.Value.Kind == DateTimeKind.Unspecified)
                    {
                        start.Value.ToUniversalTime();
                    }
                }

                if (filter.BookingDateTo != null)
                {
                    end = filter.BookingDateTo;

                    if (filter.BookingDateTo.Value.Kind == DateTimeKind.Local ||
                        filter.BookingDateTo.Value.Kind == DateTimeKind.Unspecified)
                    {
                        end.Value.ToUniversalTime();
                    }
                }

                if(filter.BookingDateFrom != null && filter.BookingDateTo != null) 
                {
                    newPred = newPred.And(x => start <= x.StartTime);
                    newPred = newPred.And(x => end >= x.EndTime);
                }

                if (filter.BookingDateFrom != null && filter.BookingDateTo == null)
                {
                    newPred = newPred.And(x => start <= x.StartTime);
                }

                if (filter.BookingDateFrom == null && filter.BookingDateTo != null)
                {
                    newPred = newPred.And(x => end >= x.EndTime);
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
                    newPred = newPred.Or(x => start <= x.CreateTime);
                    newPred = newPred.And(x => end >= x.CreateTime);
                }

                if (filter.CreateTimeFrom != null && filter.CreateTimeTo == null)
                {
                    newPred = newPred.And(x => start <= x.CreateTime);
                }

                if (filter.CreateTimeFrom == null && filter.CreateTimeTo != null)
                {
                    newPred = newPred.And(x => end >= x.CreateTime);
                }
            }

            if(filter.Status != null || filter.Rating != null) 
            {
                if(filter.Status != null && filter.Rating == null) 
                {
                    newPred = newPred.And(x => x.Status == (StatusEnum)filter.Status);
                }

                if (filter.Status != null && filter.Rating != null)
                {
                    newPred = newPred.And(x => x.Status == (StatusEnum)filter.Status);
                    newPred = newPred.Or(x => x.Rating == filter.Rating);
                }

                if (filter.Status == null && filter.Rating != null)
                {
                    newPred = newPred.And(x => x.Rating == filter.Rating);
                }
            }

            //Create a new expression instance
            Expression<Func<Booking, bool>>? pred = null;

            //If the Predicate Builder contains any predicate then
            //convert it to an expression
            if (!(newPred.Body.ToString().StartsWith("True") &&
            newPred.Body.ToString().EndsWith("True")))
            {
                pred = newPred;
            }

            //Get paged list of items with sort, filters, included navi props
            var returnedBookingList = await bookingRepo.GetPaginationAsync
                (pagingPayload.PageNum, pagingPayload.OrderColumn,
                pagingPayload.IsAscending, includeList, pred);

            if (returnedBookingList.Count() == 0) return 
                    GeneralResult<ObjectListPagingInfo>
                    .Error(404, "No bookings found ");

            //Get total elements when running the query
            var TotalElement = await bookingRepo.GetPagingTotalElement(pred);

            //Create new class to contain list result and paging info
            var FinalResult = new ObjectListPagingInfo();

            var BookingList = mapper.Map<List<BookingView>>(returnedBookingList);

            //Create new desired response 
            FinalResult.ObjectList = BookingList.Select(x => new 
            {
                x.Id, x.UserName, x.ZoneTypeName, x.ZoneNumber, x.FieldName,
                x.FieldAddress, x.TotalPrice, x.StartTime, x.EndTime, x.Status
            });

            FinalResult.TotalElement = TotalElement;
            FinalResult.CurrentPage = pagingPayload.PageNum;

            //Calculate total pages based on total element
            var CheckRemain = TotalElement % 20;
            var SumPage = TotalElement / 20;
            if (CheckRemain > 0) FinalResult.TotalPage = SumPage + 1;
            else FinalResult.TotalPage = SumPage;

            return GeneralResult<ObjectListPagingInfo>.Success(FinalResult);
        }

        public async Task<GeneralResult<ObjectListPagingInfo>> 
            RetrieveBookingsListForUser
            (PagingPayload pagingPayload, BookingPredicate filter)
        {
            //Create a new instance of predicate builder used to build query predicate
            var newPred = PredicateBuilder.New<Booking>(true);

            //list of navi props to include in query
            string? includeList = "Customer,TypeZone,ZoneInfo,FieldInfo";

            //Predicates to add to query (given that any one of them isn't null) 
            newPred = newPred.And(x => x.CustomerId == filter.UserId);
            
            if (filter.ZoneTypeId != null)
            {
                newPred = newPred.And(x => x.ZoneTypeId == filter.ZoneTypeId);
            }

            if (filter.FromPrice != null || filter.ToPrice != null)
            {
                if (filter.FromPrice != null && filter.ToPrice != null)
                {
                    newPred = newPred.And(x => filter.FromPrice <= x.TotalPrice);
                    newPred = newPred.And(x => filter.ToPrice >= x.TotalPrice);
                }

                if (filter.FromPrice != null && filter.ToPrice == null)
                {
                    newPred = newPred.And(x => filter.FromPrice <= x.TotalPrice);
                }

                if (filter.FromPrice == null && filter.ToPrice != null)
                {
                    newPred = newPred.And(x => filter.ToPrice >= x.TotalPrice);
                }
            }

            if (filter.BookingDateFrom != null || filter.BookingDateTo != null)
            {
                DateTime? start = null;
                DateTime? end = null;

                if (filter.BookingDateFrom != null)
                {
                    start = filter.BookingDateFrom;

                    if (filter.BookingDateFrom.Value.Kind == DateTimeKind.Local ||
                        filter.BookingDateFrom.Value.Kind == DateTimeKind.Unspecified)
                    {
                        start.Value.ToUniversalTime();
                    }
                }

                if (filter.BookingDateTo != null)
                {
                    end = filter.BookingDateTo;

                    if (filter.BookingDateTo.Value.Kind == DateTimeKind.Local ||
                        filter.BookingDateTo.Value.Kind == DateTimeKind.Unspecified)
                    {
                        end.Value.ToUniversalTime();
                    }
                }

                if (filter.BookingDateFrom != null && filter.BookingDateTo != null)
                {
                    newPred = newPred.And(x => start <= x.StartTime);
                    newPred = newPred.And(x => end >= x.EndTime);
                }

                if (filter.BookingDateFrom != null && filter.BookingDateTo == null)
                {
                    newPred = newPred.And(x => start <= x.StartTime);
                }

                if (filter.BookingDateFrom == null && filter.BookingDateTo != null)
                {
                    newPred = newPred.And(x => end >= x.EndTime);
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
                    newPred = newPred.Or(x => start <= x.CreateTime);
                    newPred = newPred.And(x => end >= x.CreateTime);
                }

                if (filter.CreateTimeFrom != null && filter.CreateTimeTo == null)
                {
                    newPred = newPred.And(x => start <= x.CreateTime);
                }

                if (filter.CreateTimeFrom == null && filter.CreateTimeTo != null)
                {
                    newPred = newPred.And(x => end >= x.CreateTime);
                }
            }

            if (filter.Status != null || filter.Rating != null)
            {
                if (filter.Status != null && filter.Rating == null)
                {
                    newPred = newPred.And(x => x.Status == (StatusEnum)filter.Status);
                }

                if (filter.Status != null && filter.Rating != null)
                {
                    newPred = newPred.And(x => x.Status == (StatusEnum)filter.Status);
                    newPred = newPred.Or(x => x.Rating == filter.Rating);
                }

                if (filter.Status == null && filter.Rating != null)
                {
                    newPred = newPred.And(x => x.Rating == filter.Rating);
                }
            }
            //Create a new expression instance
            Expression<Func<Booking, bool>>? pred = null;
            //If the Predicate Builder contains any predicate then
            //convert it to an expression
            if (!(newPred.Body.ToString().StartsWith("True") &&
            newPred.Body.ToString().EndsWith("True")))
            {
                pred = newPred;
            }
            //Get paged list of items with sort, filters, included navi props
            var returnedBookingList = await bookingRepo.GetPaginationAsync
                (pagingPayload.PageNum, pagingPayload.OrderColumn,
                pagingPayload.IsAscending, includeList, pred);

            if (returnedBookingList.Count() == 0) return 
                    GeneralResult<ObjectListPagingInfo>
                    .Error(404, "No bookings found for User with Id:"+ filter.UserId);

            //Get total elements when running the query
            var TotalElement = await bookingRepo.GetPagingTotalElement(pred);

            //Create new class to contain list result and paging info
            var Result = new ObjectListPagingInfo();

            var BookingList = mapper.Map<List<BookingView>>(returnedBookingList);

            //Create new desired response 
            Result.ObjectList = BookingList.Select(x => new
            {
                x.Id, x.ZoneTypeName, x.ZoneNumber, x.FieldName, x.FieldAddress,
                x.TotalPrice, x.StartTime, x.EndTime, x.Status
            }).ToList();

            //Calculate total pages based on total element
            Result.TotalElement = TotalElement;
            Result.CurrentPage = pagingPayload.PageNum;

            var CheckRemain = TotalElement % 20;
            var SumPage = TotalElement / 20;
            if (CheckRemain > 0) Result.TotalPage = SumPage + 1;
            else Result.TotalPage = SumPage;

            //var FinalList = mapper.Map<List<BookingView2>>(returnedBookingList);

            return GeneralResult<ObjectListPagingInfo>.Success(Result);
        }

        public async Task<GeneralResult<Booking>> UpdateABooking(int Id,
            BookingUpdatePayload newBookingInfo)
        {
            //Get the requested booking details for update
            var toUpdateBooking = await bookingRepo.GetById(Id);

            if (toUpdateBooking == null) return GeneralResult<Booking>.Error(
                404, "No booking found with Id:" + Id);

            //Mapping new booking info to returned booking and update
            mapper.Map(newBookingInfo ,toUpdateBooking);
         
            bookingRepo.Update(toUpdateBooking);
            await bookingRepo.SaveAsync();

            return GeneralResult<Booking>.Success(toUpdateBooking);
        }

        public async Task<GeneralResult<Booking>> UpdateABookingZoneId(int Id, int ZoneId)
        {
            //Get the requested booking details for zone Id update
            var toUpdateBookingZoneId = await bookingRepo.GetById(Id);

            if (toUpdateBookingZoneId == null) return GeneralResult<Booking>.Error(
                404, "No booking found with Id:" + Id);

            //Change zone Id and update
            toUpdateBookingZoneId.ZoneId = ZoneId;

            bookingRepo.Update(toUpdateBookingZoneId);
            await bookingRepo.SaveAsync();

            return GeneralResult<Booking>.Success(toUpdateBookingZoneId);
        }

        public async Task<GeneralResult<BookingView>> UpdateBookingStatusForUser(int Id,
            StatusEnum newStatus)
        {
            //Get requested booking details to update status
            var toUpdateBookingStatus = await bookingRepo.GetById(Id);

            if (toUpdateBookingStatus == null) return GeneralResult<BookingView>.Error(
                404, "No booking found with Id:" + Id);

            //Change status and update the booking
            toUpdateBookingStatus.Status = newStatus;

            bookingRepo.Update(toUpdateBookingStatus);
            await bookingRepo.SaveAsync();

            var updatedBooking = mapper.Map<BookingView>(toUpdateBookingStatus);

            return GeneralResult<BookingView>.Success(updatedBooking);
        }
    }
}
