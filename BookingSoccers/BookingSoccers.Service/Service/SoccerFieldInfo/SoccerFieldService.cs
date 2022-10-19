﻿using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO.Booking;
using BookingSoccers.Service.Models.DTO.ImageFolder;
using BookingSoccers.Service.Models.DTO.PriceItem;
using BookingSoccers.Service.Models.DTO.PriceMenu;
using BookingSoccers.Service.Models.DTO.SoccerField;
using BookingSoccers.Service.Models.DTO.Zone;
using BookingSoccers.Service.Models.DTO.ZoneSlot;
using BookingSoccers.Service.Models.Payload.Booking;
using BookingSoccers.Service.Models.Payload.SoccerField;
using Firebase.Auth;
using Firebase.Storage;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Npgsql.PostgresTypes.PostgresCompositeType;

namespace BookingSoccers.Service.Service.SoccerFieldInfo
{
    public class SoccerFieldService : ISoccerFieldService
    {
        
        private readonly ISoccerFieldRepo soccerFieldRepo;
        private readonly IPriceMenuRepo priceMenuRepo;
        private readonly IImageFolderRepo imageFolderRepo;
        private readonly IZoneRepo zoneRepo;
        private readonly IZoneTypeRepo zoneTypeRepo;
        private readonly IZoneSlotRepo zoneSlotRepo;
        private readonly IBookingRepo bookingRepo;
        private readonly IPaymentRepo paymentRepo;
        private readonly IUserRepo userRepo;
        private readonly IMapper mapper;

        public SoccerFieldService(ISoccerFieldRepo soccerFieldRepo,
            IMapper mapper, IZoneRepo zoneRepo, IZoneSlotRepo zoneSlotRepo,
            IPriceMenuRepo priceMenuRepo, IBookingRepo bookingRepo,
            IZoneTypeRepo zoneTypeRepo, IUserRepo userRepo, IPaymentRepo paymentRepo,
            IImageFolderRepo imageFolderRepo)
        {
            this.soccerFieldRepo = soccerFieldRepo;
            this.mapper = mapper;
            this.zoneRepo = zoneRepo;
            this.zoneSlotRepo = zoneSlotRepo;
            this.priceMenuRepo = priceMenuRepo;
            this.bookingRepo = bookingRepo;
            this.userRepo = userRepo;
            this.zoneTypeRepo = zoneTypeRepo;
            this.paymentRepo = paymentRepo;
            this.imageFolderRepo = imageFolderRepo;
        }

        public async Task<GeneralResult<PreBookingInfo>> CheckZonesAndCalculatePrice
            (BookingValidateForm createFormInfo)
        {
            var ZoneList = await zoneRepo.getZonesByZoneType
                (createFormInfo.FieldId, createFormInfo.ZoneTypeId);

            if (ZoneList.Count == 0) return GeneralResult<PreBookingInfo>.Error
                    (404, "No zone found for this zone type");

            List<int> ZoneIdList = ZoneList
                .Select(x => x.Id)
                .ToList();

            PreBookingInfo result = null;

            var FieldOpeningTime = await soccerFieldRepo.GetById(createFormInfo.FieldId);

            var LocalTime = createFormInfo.BookingDate.ToLocalTime();

            var StartTime = new DateTime
                    (LocalTime.Year, LocalTime.Month,
                    LocalTime.Day, createFormInfo.StartTimeHour,
                    createFormInfo.StartTimeMinute, 0);

            var EndTime = StartTime.AddMinutes(createFormInfo.HireAmount);

            if (!(FieldOpeningTime.OpenHour <= StartTime.TimeOfDay &&
                StartTime.TimeOfDay <= FieldOpeningTime.CloseHour) ||
                !(FieldOpeningTime.OpenHour <= EndTime.TimeOfDay &&
                EndTime.TimeOfDay <= FieldOpeningTime.CloseHour))
                return GeneralResult<PreBookingInfo>.Error
                    (400, "Invalid hire start time or end time");

            var FieldResult = await soccerFieldRepo.GetById(createFormInfo.FieldId);

            var ZoneTypeResult = await zoneTypeRepo.GetById(createFormInfo.ZoneTypeId);

            foreach (var it in ZoneIdList) 
            {
                var ZoneSlotList = await zoneSlotRepo.getZoneSlotsByZoneId
                (it, createFormInfo.BookingDate);

                var FinalZoneSlotList = ZoneSlotList
                    .Where(x => x.StartTime.ToLocalTime().TimeOfDay >= StartTime.TimeOfDay &&
                    x.StartTime.ToLocalTime().TimeOfDay < EndTime.TimeOfDay)
                    .OrderBy(x => x.StartTime)
                    .ToList();

                var SlotCount = FinalZoneSlotList.Count;

                var TimeAmountCheck = 30 * SlotCount;

                if (TimeAmountCheck != createFormInfo.HireAmount) return
                GeneralResult<PreBookingInfo>.Error(400, "No slots available from " +
                createFormInfo.StartTimeHour +"h" +createFormInfo.StartTimeMinute);

                List<int> SlotIdList = new List<int>();

                foreach(var i in FinalZoneSlotList) 
                {
                    SlotIdList.Add(i.Id);
                }

                var PriceMenu = await priceMenuRepo.GetAPriceMenu
           (createFormInfo.FieldId, createFormInfo.BookingDate, createFormInfo.ZoneTypeId);

                if (PriceMenu == null) return GeneralResult<PreBookingInfo>.Error
                        (404, "No price menu to book");

                var PriceItemList = PriceMenu.PriceItems
                    .OrderBy(x => x.StartTime)
                    .ToList();

                int Price = 0;

                foreach (var item in PriceItemList)
                {
                    if (item.StartTime <= StartTime.TimeOfDay &&
                        StartTime.TimeOfDay <= item.EndTime &&
                        item.StartTime <= EndTime.TimeOfDay &&
                        EndTime.TimeOfDay <= item.EndTime)
                    {
                        Price = (int)(item.Price * (double)createFormInfo.HireAmount / 60);
                        break;
                    }

                    if (StartTime.TimeOfDay <= item.StartTime &&
                        item.StartTime <= EndTime.TimeOfDay &&
                        StartTime.TimeOfDay <= item.EndTime &&
                        item.EndTime <= EndTime.TimeOfDay)
                    {
                        var priceItemMinute = (item.EndTime - item.StartTime).Hours * 60 +
                            (item.EndTime - item.StartTime).Minutes;
                        Price += (int)(item.Price * (double)priceItemMinute / 60);
                        continue;
                    }

                    if (item.StartTime < StartTime.TimeOfDay &&
                        StartTime.TimeOfDay < item.EndTime &&
                        item.EndTime < EndTime.TimeOfDay)
                    {
                        var priceItemMinute = (item.EndTime - StartTime.TimeOfDay).Hours * 60 +
                            (item.EndTime - StartTime.TimeOfDay).Minutes;
                        Price += (int)(item.Price * (double)priceItemMinute / 60);
                        continue;
                    }

                    if (item.StartTime < EndTime.TimeOfDay &&
                        EndTime.TimeOfDay < item.EndTime &&
                        item.StartTime > StartTime.TimeOfDay)
                    {
                        var priceItemMinute = (EndTime.TimeOfDay - item.StartTime).Hours * 60 +
                            (EndTime.TimeOfDay - item.StartTime).Minutes;
                        Price += (int)(item.Price * (double)priceItemMinute / 60);
                        continue;
                    }
                }

                result = new PreBookingInfo()
                {
                    StartTime = StartTime.TimeOfDay,
                    EndTime = EndTime.TimeOfDay,
                    HireDate = createFormInfo.BookingDate,
                    TotalPrice = Price,
                    Address = createFormInfo.Address,
                    FieldName = FieldResult.FieldName,
                    ZoneType = ZoneTypeResult.Name,
                    ZoneId = it,
                    SlotsIdList =SlotIdList,
                    DepositPercent = ZoneTypeResult.DepositPercent,
                    PrepayAmount = Price/100 * ZoneTypeResult.DepositPercent
                };
                break;
            }

            return GeneralResult<PreBookingInfo>.Success(result);
        }

        public async Task<GeneralResult<SoccerField>> AddANewSoccerField(
            SoccerFieldCreatePayload SoccerFieldinfo)
        {
            var soccerFieldExistCheck = await soccerFieldRepo.Get().Where(x =>
            x.FieldName == SoccerFieldinfo.FieldName ||
            x.Address == SoccerFieldinfo.Address).FirstOrDefaultAsync();

            if (soccerFieldExistCheck != null) return
                GeneralResult<SoccerField>.Error(
                    409, "Soccer field name or address already exists");

            var toCreateSoccerField = mapper.Map<SoccerField>(SoccerFieldinfo);
            toCreateSoccerField.OpenHour = new TimeSpan
                (SoccerFieldinfo.OpenHour, SoccerFieldinfo.OpenMinute, 0);

            toCreateSoccerField.CloseHour = new TimeSpan
                (SoccerFieldinfo.CloseHour, SoccerFieldinfo.CloseMinute, 0);

            soccerFieldRepo.Create(toCreateSoccerField);
            await soccerFieldRepo.SaveAsync();

            var newImageFolder = new ImageFolder()
            {
                FieldId = toCreateSoccerField.Id,
                Path = SoccerFieldinfo.ImageFolderPath
            };

            imageFolderRepo.Create(newImageFolder);
            await imageFolderRepo.SaveAsync();
            
            return GeneralResult<SoccerField>.Success(toCreateSoccerField);
        }

        public async Task<GeneralResult<SoccerFieldView1>> GetFieldForUserByFieldID
            (int FieldId)
        {
            var SoccerField =
                await soccerFieldRepo.GetSoccerFieldByFieldId(FieldId);

            if (SoccerField == null)
                return GeneralResult<SoccerFieldView1>.Error(
                404, "No soccer fields found for UserId:" + FieldId);

            List<PriceItemView>? priceItemViewList = null;
            List<PriceMenuView>? priceMenuViewsList = null;

            var SoccerFieldViewItem = mapper.Map<SoccerFieldView1>(SoccerField);

            var FieldImageList = mapper.Map<List<ImageList>>(SoccerField.ImageList);

            SoccerFieldViewItem.ImageList = FieldImageList;

            if(SoccerField.ReviewScoreSum > 0 && SoccerField.TotalReviews > 0)
                SoccerFieldViewItem.AverageReviewScore =
                    SoccerField.ReviewScoreSum / SoccerField.TotalReviews;

            if (SoccerField.ReviewScoreSum == 0 && SoccerField.TotalReviews == 0)
                SoccerFieldViewItem.AverageReviewScore = 0;

            if (SoccerField.PriceMenus.Count > 0)
            {
                priceMenuViewsList = new List<PriceMenuView>();
                foreach (var item in SoccerField.PriceMenus)
                {
                    var priceMenu = mapper.Map<PriceMenuView>(item);
                    if (item.PriceItems != null)
                    {
                        priceItemViewList = new List<PriceItemView>();
                        foreach (var it in item.PriceItems)
                        {
                            var priceItem = mapper.Map<PriceItemView>(it);
                            priceItemViewList.Add(priceItem);
                        }
                        priceMenu.PriceItemsList = priceItemViewList;
                    }
                    priceMenuViewsList.Add(priceMenu);
                }
                SoccerFieldViewItem.PriceMenusList = priceMenuViewsList;
            }
            else
                SoccerFieldViewItem.PriceMenusList = null;
            

            var ZoneList = await zoneRepo.getFieldZonesByFieldId(FieldId);

            if(ZoneList.Count > 0)
            {
                List<ZoneView> FinalZoneViewList = new List<ZoneView>();
                foreach (var item in ZoneList)
                {
                    var SlotList = await zoneSlotRepo.getZoneSlots(item.Id, DateTime.UtcNow);

                    ZoneView ZoneViewItem = new ZoneView();
                    ZoneViewItem.ZoneNumber = item.Number;
                    ZoneViewItem.ZoneType = item.ZoneTypeId;
                    List<ZoneSlotView> SlotViewList = new List<ZoneSlotView>();
                    foreach (var it in SlotList) 
                    {

                        ZoneSlotView SlotViewItem = new ZoneSlotView()
                        {
                            SlotStartTime = it.StartTime.ToLocalTime().TimeOfDay,
                            SlotEndTime = it.EndTime.ToLocalTime().TimeOfDay
                        };

                        SlotViewList.Add(SlotViewItem);
                    }
                    var FinalList = SlotViewList.OrderBy(x => x.SlotStartTime).ToList();
                    
                    ZoneViewItem.ZoneTypeSlots = SlotViewList;
                    FinalZoneViewList.Add(ZoneViewItem);
                }

                var FilterFinalList = FinalZoneViewList.OrderBy(x => x.ZoneType).ToList();

                SoccerFieldViewItem.ZonesList = FilterFinalList;
            }
            else 
                SoccerFieldViewItem.ZonesList = null;

            return GeneralResult<SoccerFieldView1>.Success(SoccerFieldViewItem);
        }

        public async Task<GeneralResult<SoccerFieldView2>> GetFieldScheduleOfADateById
            (int FieldId, DateTime date)
        {
            var soccerFieldSchedule = await soccerFieldRepo
                .GetFieldBookingScheduleOfADateByFieldId(FieldId, date);

            if (soccerFieldSchedule != null) return
                GeneralResult<SoccerFieldView2>.Error(
                    404, "No soccer field schedule found for date: "+date);

            var FinalSoccerField = mapper.Map<SoccerFieldView2>(soccerFieldSchedule);

            var BookingViewList = mapper
                .Map<List<BookingView1>>(soccerFieldSchedule.Bookings);

            var ZoneViewList = mapper
                .Map<List<ZoneView1>>(soccerFieldSchedule.Zones);

            FinalSoccerField.Bookings = BookingViewList;
            FinalSoccerField.Zones = ZoneViewList;

            return GeneralResult<SoccerFieldView2>.Success(FinalSoccerField);
        }

        public async Task<GeneralResult<List<SoccerFieldView3>>> 
            GetFieldsForManagerByManagerId(int ManagerId)
        {
            var FieldsList = await soccerFieldRepo
                .GetFieldsForManagerByManagerId(ManagerId);

            if (FieldsList.Count == 0) return 
                    GeneralResult<List<SoccerFieldView3>>.Error(
                404, "No soccer fields found with manager Id:" + ManagerId);

            List<SoccerFieldView3> FinalList = new List<SoccerFieldView3>();

            foreach(var item in FieldsList) 
            {
                var FieldItem = mapper.Map<SoccerFieldView3>(item);
                List<ImageList> imageList = new List<ImageList>();
                if (item.ImageList.Count > 0) 
                {
                    imageList = mapper.Map<List<ImageList>>(item.ImageList.ToList());
                    FieldItem.ImageList = imageList;
                }
                FinalList.Add(FieldItem);
            }
            
            return GeneralResult<List<SoccerFieldView3>>.Success(FinalList);
        }

        public async Task<GeneralResult<List<PriceMenuView>>> 
            GetPriceMenusForManagerByFieldId
            (int FieldId)
        {
            var returnedPriceMenusList = await priceMenuRepo
                .GetPriceMenusForAField(FieldId);

            if(returnedPriceMenusList.Count == 0) return
                    GeneralResult<List<PriceMenuView>>.Error(
                404, "No price menus found with field Id:" + FieldId);

            List<PriceMenuView> FinalPriceMenusList = new List<PriceMenuView>();

            foreach(var item in returnedPriceMenusList) 
            {
                var priceMenuView = mapper.Map<PriceMenuView>(item);
                if(item.PriceItems != null) 
                {
                    List<PriceItemView> priceItemList = new List<PriceItemView>();
                    foreach(var it in item.PriceItems) 
                    {
                        var FinalPriceItemsList =
                        mapper.Map<PriceItemView>(it);
                        priceItemList.Add(FinalPriceItemsList);
                    }
                    priceMenuView.PriceItemsList = priceItemList;
                }
                FinalPriceMenusList.Add(priceMenuView);
            }

            return GeneralResult<List<PriceMenuView>>.Success(FinalPriceMenusList);
        }

        public async Task<GeneralResult<List<FieldRegularCustomerView>>> 
            GetRegularCustomerList(int FieldId)
        {
            var BookingList = await bookingRepo.GetBookingsByFieldId(FieldId);

            if (BookingList.Count == 0) return
                    GeneralResult<List<FieldRegularCustomerView>>
                    .Error(404, "Field with Id:"+FieldId+" has no regular customer");

            var DistinctBookingList = await bookingRepo
                .GetBookingsDistinctByFieldId(FieldId);

            int count = 0;

            List<FieldRegularCustomerView> RegularCustomerList =
                new List<FieldRegularCustomerView>();

            foreach (var booking in DistinctBookingList) 
            {
                count = 0;
                FieldRegularCustomerView RegularCustomer = new FieldRegularCustomerView();
                foreach(var item in BookingList) 
                { 
                    if(booking.CustomerId == item.CustomerId) 
                    {
                        count += 1;
                    }
                }

                if (count > 3) 
                {
                    RegularCustomer.UserName = booking.Customer.UserName;
                    RegularCustomer.Phone = booking.Customer.PhoneNumber;
                    RegularCustomer.BookingCount = count;
                    RegularCustomerList.Add(RegularCustomer);
                }
            }

            return GeneralResult<List<FieldRegularCustomerView>>
                .Success(RegularCustomerList);
        }

        public async Task<GeneralResult<SoccerFieldSalesReport>> 
            GetSalesReportForField(int FieldId)
        {
            SoccerFieldSalesReport SalesReport = new SoccerFieldSalesReport();

            var UpperMonthEndDate = DateTime.Now;

            var currentMonthDays = DateTime.DaysInMonth(DateTime.Now.Month, DateTime.Now.Year);

            var Days = currentMonthDays - DateTime.Now.Day;

            if (Days > 0)
                UpperMonthEndDate = DateTime.Now.AddDays(Days);

            var Last6MonthDate = UpperMonthEndDate.AddMonths(-6);

            DateTime LowerMonthStartDate = new DateTime();

            var Days1 = Last6MonthDate.Day - 1;

            if(Days1 > 0) 
                LowerMonthStartDate = Last6MonthDate.AddDays(-Days1);

            var Past6MonthSalesReport = await
        bookingRepo.GetBookingsForReport(FieldId, UpperMonthEndDate, LowerMonthStartDate);

            List<Booking6MonthReportView> HalfYearReportList =
                new List<Booking6MonthReportView>();

            Booking6MonthReportView HalfYearReportItem =
                new Booking6MonthReportView();

            int CurrentMonth = 0;
            int PreviousMonth = 0;

            if (Past6MonthSalesReport.Count > 0)
            {
                foreach (var item in Past6MonthSalesReport)
                {
                    if (CurrentMonth == 0 && PreviousMonth == 0)
                    {
                        CurrentMonth = item.StartTime.Month;
                        PreviousMonth = item.StartTime.Month;

                        HalfYearReportItem.Year = item.StartTime.Year;
                        HalfYearReportItem.Month = item.StartTime.Month;
                        HalfYearReportItem.SalesSum += item.TotalPrice;
                        HalfYearReportItem.BookingCount += 1;

                        continue;
                    }

                    CurrentMonth = item.StartTime.Month;

                    if (CurrentMonth == PreviousMonth)
                    {
                        HalfYearReportItem.SalesSum += item.TotalPrice;
                        HalfYearReportItem.BookingCount += 1;
                        continue;
                    }

                    if (CurrentMonth != PreviousMonth)
                    {
                        HalfYearReportList.Add(HalfYearReportItem);

                        HalfYearReportItem = new Booking6MonthReportView();

                        PreviousMonth = item.StartTime.Month;

                        HalfYearReportItem.Year = item.StartTime.Year;
                        HalfYearReportItem.Month = item.StartTime.Month;
                        HalfYearReportItem.SalesSum += item.TotalPrice;
                        HalfYearReportItem.BookingCount += 1;
                    }

                    if(item.Id == Past6MonthSalesReport.Last().Id)
                        HalfYearReportList.Add(HalfYearReportItem);

                }
            }
            else HalfYearReportList = null;

            SalesReport.Booking6MonthReports = HalfYearReportList;

            var UpperDayLimit = DateTime.Now.AddDays(7);
            var LowerDayLimit = DateTime.Now.AddDays(-7);

            var Past14DaysSalesReport =
                await bookingRepo.GetBookingsForReport(FieldId, UpperDayLimit, LowerDayLimit);

            List<Booking14DaysReportView> Booking14DaysReportList =
                new List<Booking14DaysReportView>();

            Booking14DaysReportView Booking14DaysReportItem = new Booking14DaysReportView();

            int CurrentDay = 0;
            int PreviousDay = 0;

            if (Past14DaysSalesReport.Count > 0) 
            {
                foreach (var item in Past14DaysSalesReport)
                {
                    if (CurrentDay == 0 && PreviousDay == 0)
                    {
                        CurrentDay = item.StartTime.Day;
                        PreviousDay = item.StartTime.Day;

                        Booking14DaysReportItem.Month = item.StartTime.Month;
                        Booking14DaysReportItem.Day = item.StartTime.Day;
                        Booking14DaysReportItem.DaySalesSum += item.TotalPrice;
                        Booking14DaysReportItem.BookingCount += 1;

                        continue;
                    }

                    CurrentDay = item.StartTime.Day;

                    if (CurrentDay == PreviousDay)
                    {
                        Booking14DaysReportItem.DaySalesSum += item.TotalPrice;
                        Booking14DaysReportItem.BookingCount += 1;
                        continue;
                    }

                    if (CurrentDay != PreviousDay)
                    {
                        Booking14DaysReportList.Add(Booking14DaysReportItem);

                        Booking14DaysReportItem = new Booking14DaysReportView();

                        PreviousDay = item.StartTime.Day;

                        Booking14DaysReportItem.Month = item.StartTime.Month;
                        Booking14DaysReportItem.Day = item.StartTime.Day;
                        Booking14DaysReportItem.DaySalesSum += item.TotalPrice;
                        Booking14DaysReportItem.BookingCount += 1;
                    }

                    if (item.Id == Past14DaysSalesReport.Last().Id)
                        Booking14DaysReportList.Add(Booking14DaysReportItem);
                }
            }else Booking14DaysReportList = null;

            SalesReport.Booking14DaysReport = Booking14DaysReportList;


            var LastMonthBooking = await bookingRepo.GetBookingsInLastMonth(FieldId);

            CustomBookingReportView LastMonthReport = new CustomBookingReportView();

            if (LastMonthBooking.Count > 0)
            {
                LastMonthReport.BookingCount = LastMonthBooking.Count;
                LastMonthReport.TotalIncomeCount = LastMonthBooking.Sum(x => x.TotalPrice);
            }
            else LastMonthReport = null;

            SalesReport.CustomBookingReport = LastMonthReport;

            return GeneralResult<SoccerFieldSalesReport>.Success(SalesReport);

        }

        public async Task<GeneralResult<SoccerField>> RemoveASoccerField(int SoccerFieldId)
        {
            var toDeleteSoccerField = await soccerFieldRepo.GetById(SoccerFieldId);

            if (toDeleteSoccerField == null) return GeneralResult<SoccerField>.Error(
                404, "No soccer field found with Id:" + SoccerFieldId);

            soccerFieldRepo.Delete(toDeleteSoccerField);
            await soccerFieldRepo.SaveAsync();

            return GeneralResult<SoccerField>.Success(toDeleteSoccerField);
        }

        public async Task<GeneralResult<List<SoccerField>>> RetrieveAllSoccerFields()
        {
            var soccerFieldList = await soccerFieldRepo.Get().ToListAsync();

            if (soccerFieldList.Count == 0) return GeneralResult<List<SoccerField>>.Error(
                404, "No soccer fields found");

            return GeneralResult<List<SoccerField>>.Success(soccerFieldList); 
        }

        public async Task<GeneralResult<List<SoccerFieldListView>>> 
            RetrieveAllSoccerFieldsForUser()
        {
            var soccerFieldList = await soccerFieldRepo.Get().ToListAsync();

            if (soccerFieldList.Count == 0) return GeneralResult<List<SoccerFieldListView>>.Error(
                404, "No soccer fields found");

            var resultList = mapper.Map<List<SoccerFieldListView>>(soccerFieldList);

            return GeneralResult<List<SoccerFieldListView>>.Success(resultList);
        }

        public async Task<GeneralResult<SoccerField>> RetrieveASoccerFieldById(int SoccerFieldId)
        {
            var retrievedSoccerField = await soccerFieldRepo.GetById(SoccerFieldId);

            if (retrievedSoccerField == null) return GeneralResult<SoccerField>.Error(
                404, "No soccer field found with Id:"+ SoccerFieldId);

            return GeneralResult<SoccerField>.Success(retrievedSoccerField);
        }

        public async Task<GeneralResult<SoccerField>> UpdateASoccerField(int Id,
            SoccerFieldUpdatePayload newSoccerFieldInfo)
        {
            var toUpdateSoccerField = await soccerFieldRepo.GetById(Id);

            if (toUpdateSoccerField == null) return GeneralResult<SoccerField>.Error(
                404, "No soccer field found with Id:" + Id);

            mapper.Map(newSoccerFieldInfo, toUpdateSoccerField);

            var newOpenHour = new TimeSpan
                (newSoccerFieldInfo.OpenTimeHour, newSoccerFieldInfo.OpenTimeMinute, 0);

            var newCloseHour = new TimeSpan
                (newSoccerFieldInfo.CloseTimeHour, newSoccerFieldInfo.CloseTimeMinute, 0);

            toUpdateSoccerField.OpenHour = newOpenHour;
            toUpdateSoccerField.CloseHour = newCloseHour;

            soccerFieldRepo.Update(toUpdateSoccerField);
            await soccerFieldRepo.SaveAsync();

            return GeneralResult<SoccerField>.Success(toUpdateSoccerField);
        }

        public async Task<GeneralResult<AddedBookingView>> AddANewBooking
            (BookingCreateForm bookingInfo)
        {

            var StartTime = new DateTime(bookingInfo.HireDate.Year,
                bookingInfo.HireDate.Month, bookingInfo.HireDate.Day,
                bookingInfo.StartTimeHour, bookingInfo.StartTimeMinute,
                0);

            var EndTime = new DateTime(bookingInfo.HireDate.Year,
                bookingInfo.HireDate.Month, bookingInfo.HireDate.Day,
                bookingInfo.EndTimeHour, bookingInfo.EndTimeMinute,
                0);

            var CheckBookingExist = await bookingRepo
                .CheckBookingDuplicate(StartTime.ToUniversalTime(), EndTime.ToUniversalTime());

            if (CheckBookingExist != null) return GeneralResult<AddedBookingView>
                    .Error(409, "Can not book, another booking with overlapping slots found");

            List<ZoneSlot> ZoneList = new List<ZoneSlot>();
            foreach(var item in bookingInfo.SlotsIdList) 
            {
                var ZoneSlot = await zoneSlotRepo.GetById(item);
                ZoneSlot.Status = 1;
                ZoneList.Add(ZoneSlot);
            }

            zoneSlotRepo.BulkUpdate(ZoneList);
            await zoneSlotRepo.SaveAsync();

            var User = await userRepo.GetByUserName(bookingInfo.UserName);
            var ZoneType = await zoneTypeRepo.GetZoneTypeByName(bookingInfo.ZoneType);
            var Field = await soccerFieldRepo.GetFieldByFieldName(bookingInfo.FieldName);

            Booking BookingItem = new Booking()
            {
                CustomerId = User.Id,
                ZoneTypeId = ZoneType.Id,
                ZoneId = bookingInfo.ZoneId,
                FieldId = Field.Id,
                TotalPrice = bookingInfo.TotalPrice,
                StartTime = StartTime.ToUniversalTime(),
                EndTime = EndTime.ToUniversalTime(),
                CreateTime = DateTime.UtcNow,
                Status = StatusEnum.Waiting,
                Rating = 0
            };

            bookingRepo.Create(BookingItem);
            await bookingRepo.SaveAsync();

            Payment newPayment = new Payment()
            {
                BookingId = BookingItem.Id,
                ReceiverId = Field.ManagerId,
                Type = PaymentTypeEnum.Deposit,
                Amount = bookingInfo.PrepayAmount,
                Time = DateTime.UtcNow
            };

            paymentRepo.Create(newPayment);
            await paymentRepo.SaveAsync();

            var ZoneItem = await zoneRepo.GetById(bookingInfo.ZoneId);
            var LocalStartTime = StartTime.ToLocalTime();
            var LocalEndTime = EndTime.ToLocalTime();

            AddedBookingView AddedBooking = new AddedBookingView()
            {
                UserName = bookingInfo.UserName,
                ZoneTypeName = bookingInfo.ZoneType,
                ZoneNumber = ZoneItem.Number,
                FieldName = Field.FieldName,
                FieldAddress = Field.Address,
                TotalPrice = bookingInfo.TotalPrice,
                StartTime = LocalStartTime,
                EndTime = LocalEndTime,
                CreateTime = DateTime.Now,
                Status = StatusEnum.Waiting,
                DepositPercent = ZoneType.DepositPercent,
                DepositAmount = bookingInfo.PrepayAmount,
                PayCreateTime = DateTime.Now
            };

            return GeneralResult<AddedBookingView>.Success(AddedBooking);
        }
    }
}
