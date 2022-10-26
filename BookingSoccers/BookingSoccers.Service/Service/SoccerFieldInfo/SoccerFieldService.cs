using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
using BookingSoccers.Repo.Repository.SoccerFieldInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.DTO.Booking;
using BookingSoccers.Service.Models.DTO.ImageFolder;
using BookingSoccers.Service.Models.DTO.PriceItem;
using BookingSoccers.Service.Models.DTO.PriceMenu;
using BookingSoccers.Service.Models.DTO.SoccerField;
using BookingSoccers.Service.Models.DTO.Zone;
using BookingSoccers.Service.Models.DTO.ZoneSlot;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.Booking;
using BookingSoccers.Service.Models.Payload.SoccerField;
using Firebase.Auth;
using Firebase.Storage;
using FirebaseAdmin.Auth;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            //Check if zone exist
            var ZoneList = await zoneRepo.getZonesByZoneType
                (createFormInfo.FieldId, createFormInfo.ZoneTypeId);

            if (ZoneList.Count == 0) return GeneralResult<PreBookingInfo>.Error
                    (404, "No zone found for this zone type");

            List<int> ZoneIdList = ZoneList
                .Select(x => x.Id)
                .ToList();

            PreBookingInfo result = null;

            //Get field opening time
            var FieldOpeningTime = await soccerFieldRepo.GetById(createFormInfo.FieldId);

            var LocalTime = createFormInfo.BookingDate.ToLocalTime();

            var StartTime = new DateTime
                    (LocalTime.Year, LocalTime.Month,
                    LocalTime.Day, createFormInfo.StartTimeHour,
                    createFormInfo.StartTimeMinute, 0);

            var EndTime = StartTime.AddMinutes(createFormInfo.HireAmount);

            //Check if user request's start or end time is
            //valid (within field opening hour)
            if (!(FieldOpeningTime.OpenHour <= StartTime.TimeOfDay &&
                StartTime.TimeOfDay <= FieldOpeningTime.CloseHour) ||
                !(FieldOpeningTime.OpenHour <= EndTime.TimeOfDay &&
                EndTime.TimeOfDay <= FieldOpeningTime.CloseHour))
                return GeneralResult<PreBookingInfo>.Error
                    (400, "Invalid hire start time or end time");

            var FieldResult = await soccerFieldRepo.GetById(createFormInfo.FieldId);

            var ZoneTypeResult = await zoneTypeRepo.GetById(createFormInfo.ZoneTypeId);

            //for each zone of requested zone type
            foreach (var it in ZoneIdList) 
            {
                var ZoneSlotList = await zoneSlotRepo.getZoneSlotsByZoneId
                (it, createFormInfo.BookingDate);

                //Get zone slots that overlaps with request start and end time
                var FinalZoneSlotList = ZoneSlotList
                    .Where(x => x.StartTime.ToLocalTime().TimeOfDay >= StartTime.TimeOfDay &&
                    x.StartTime.ToLocalTime().TimeOfDay < EndTime.TimeOfDay)
                    .OrderBy(x => x.StartTime)
                    .ToList();

                //Then check if minute sum of the
                //returned zone slots = request hire amount
                var SlotCount = FinalZoneSlotList.Count;

                var TimeAmountCheck = 30 * SlotCount;

                if (TimeAmountCheck != createFormInfo.HireAmount &&
                    !ZoneIdList.Last().Equals(it)) continue;

                if (TimeAmountCheck != createFormInfo.HireAmount && 
                    ZoneIdList.Last().Equals(it)) return
                GeneralResult<PreBookingInfo>.Error(400, "No slots available from " +
                createFormInfo.StartTimeHour +"h" +createFormInfo.StartTimeMinute);

                List<int> SlotIdList = new List<int>();

                foreach(var i in FinalZoneSlotList) 
                {
                    SlotIdList.Add(i.Id);
                }

                //Check if there is any price menu(s) for request's start or end time
                var PriceMenu = await priceMenuRepo.GetAPriceMenu
           (createFormInfo.FieldId, createFormInfo.BookingDate, createFormInfo.ZoneTypeId);

                if (PriceMenu == null) return GeneralResult<PreBookingInfo>.Error
                        (404, "No price menu to book");

                var PriceItemList = PriceMenu.PriceItems
                    .OrderBy(x => x.StartTime)
                    .ToList();

                int Price = 0;

                //calculate price for each price item that overlaps
                //the request's start or end time
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

                //Create new instance of pre booking info for user review
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
            //Check duplicate field name or address
            var soccerFieldExistCheck = await soccerFieldRepo.Get().Where(x =>
            x.FieldName == SoccerFieldinfo.FieldName ||
            x.Address == SoccerFieldinfo.Address).FirstOrDefaultAsync();

            if (soccerFieldExistCheck != null) return
                GeneralResult<SoccerField>.Error(
                    409, "Soccer field name or address already exists");

            //Mapping new soccer field info to new instance of soccerfield
            //and create
            var toCreateSoccerField = mapper.Map<SoccerField>(SoccerFieldinfo);
            toCreateSoccerField.OpenHour = new TimeSpan
                (SoccerFieldinfo.OpenHour, SoccerFieldinfo.OpenMinute, 0);

            toCreateSoccerField.CloseHour = new TimeSpan
                (SoccerFieldinfo.CloseHour, SoccerFieldinfo.CloseMinute, 0);

            soccerFieldRepo.Create(toCreateSoccerField);
            await soccerFieldRepo.SaveAsync();

            //var newImageFolder = new ImageFolder()
            //{
            //    FieldId = toCreateSoccerField.Id,
            //    Path = SoccerFieldinfo.ImageFolderPath
            //};

            //imageFolderRepo.Create(newImageFolder);
            //await imageFolderRepo.SaveAsync();
            
            return GeneralResult<SoccerField>.Success(toCreateSoccerField);
        }

        public async Task<GeneralResult<SoccerFieldView1>> GetFieldForUserByFieldID
            (int FieldId)
        {
            //Check if field exist
            var SoccerField =
                await soccerFieldRepo.GetSoccerFieldByFieldId(FieldId);

            if (SoccerField == null)
                return GeneralResult<SoccerFieldView1>.Error(
                404, "No soccer fields found for UserId:" + FieldId);

            List<PriceItemView>? priceItemViewList = null;
            List<PriceMenuView>? priceMenuViewsList = null;

            //Map returned soccer field to Soccer Field DTO
            var SoccerFieldViewItem = mapper.Map<SoccerFieldView1>(SoccerField);

            //Map returned imagelist to Image Dto
            var FieldImageList = mapper.Map<List<ImageList>>(SoccerField.ImageList);

            //and assign it Soccer Field DTO
            SoccerFieldViewItem.ImageList = FieldImageList;

            //Calculate Review Score
            if(SoccerField.ReviewScoreSum > 0 && SoccerField.TotalReviews > 0)
                SoccerFieldViewItem.AverageReviewScore =
                    SoccerField.ReviewScoreSum / SoccerField.TotalReviews;

            if (SoccerField.ReviewScoreSum == 0 && SoccerField.TotalReviews == 0)
                SoccerFieldViewItem.AverageReviewScore = 0;

            //If there are any price menu
            if (SoccerField.PriceMenus.Count > 0)
            {
                priceMenuViewsList = new List<PriceMenuView>();
                //for each menu add price item to it
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

            //If field has any zone then
            if(ZoneList.Count > 0)
            {
                List<ZoneView> FinalZoneViewList = new List<ZoneView>();
                //for each zone
                foreach (var item in ZoneList)
                {
                    var SlotList = await zoneSlotRepo.getZoneSlots(item.Id, DateTime.UtcNow);

                    //Create new instance of zone 
                    ZoneView ZoneViewItem = new ZoneView();
                    ZoneViewItem.ZoneNumber = item.Number;
                    ZoneViewItem.ZoneType = item.ZoneTypeId;
                    List<ZoneSlotView> SlotViewList = new List<ZoneSlotView>();
                    //for each zone slot add it to the zone
                    foreach (var it in SlotList) 
                    {

                        ZoneSlotView SlotViewItem = new ZoneSlotView()
                        {
                            SlotStartTime = it.StartTime.ToLocalTime().TimeOfDay,
                            SlotEndTime = it.EndTime.ToLocalTime().TimeOfDay,
                            Status = it.Status
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
            //Get Bookings of a field in a certain date using fieldId 
            var soccerFieldSchedule = await soccerFieldRepo
                .GetFieldBookingScheduleOfADateByFieldId(FieldId, date);

            if (soccerFieldSchedule != null) return
                GeneralResult<SoccerFieldView2>.Error(
                    404, "No soccer field schedule found for date: "+date);

            //Mapping returned result to Soccer Field DTO
            var FinalSoccerField = mapper.Map<SoccerFieldView2>(soccerFieldSchedule);

            //Mapping result extracted bookings to a list 
            var BookingViewList = mapper
                .Map<List<BookingView1>>(soccerFieldSchedule.Bookings);

            //Mapping result extracted zones to a list
            var ZoneViewList = mapper
                .Map<List<ZoneView1>>(soccerFieldSchedule.Zones);

            //Assign booking list to DTO
            FinalSoccerField.Bookings = BookingViewList;

            //Assign zone list to DTO
            FinalSoccerField.Zones = ZoneViewList;

            return GeneralResult<SoccerFieldView2>.Success(FinalSoccerField);
        }

        public async Task<GeneralResult<List<SoccerFieldView3>>> 
            GetFieldsForManagerByManagerId(int ManagerId)
        {
            //Get fields owned by a manager by manager Id
            var FieldsList = await soccerFieldRepo
                .GetFieldsForManagerByManagerId(ManagerId);

            if (FieldsList.Count == 0) return 
                    GeneralResult<List<SoccerFieldView3>>.Error(
                404, "No soccer fields found with manager Id:" + ManagerId);

            List<SoccerFieldView3> FinalList = new List<SoccerFieldView3>();

            //for each field in returned list
            foreach(var item in FieldsList) 
            {
                //Map to DTO
                var FieldItem = mapper.Map<SoccerFieldView3>(item);

                List<ImageList> imageList = new List<ImageList>();
                
                //if field has any imgs
                if (item.ImageList.Count > 0)
                { 
                    //then map image folders of the field to list
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
            //Get price menus for a field by field Id
            var returnedPriceMenusList = await priceMenuRepo
                .GetPriceMenusForAField(FieldId);

            if(returnedPriceMenusList.Count == 0) return
                    GeneralResult<List<PriceMenuView>>.Error(
                404, "No price menus found with field Id:" + FieldId);

            List<PriceMenuView> FinalPriceMenusList = new List<PriceMenuView>();

            //For each price menu in returned list
            foreach(var item in returnedPriceMenusList) 
            {
                var priceMenuView = mapper.Map<PriceMenuView>(item);
                //if the current price menu has price items
                if(item.PriceItems != null) 
                {
                    List<PriceItemView> priceItemList = new List<PriceItemView>();
                    //Then add price items to the price menu 
                    foreach (var it in item.PriceItems) 
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
            //Get bookings of a field by field id
            var BookingList = await bookingRepo.GetBookingsByFieldId(FieldId);

            if (BookingList.Count == 0) return
                    GeneralResult<List<FieldRegularCustomerView>>
                    .Error(404, "Field with Id:"+FieldId+" has no booking");

            //Get list of field bookings distinct by Customer Id
            var DistinctBookingList = await bookingRepo
                .GetBookingsDistinctByCustomerId(FieldId);

            int count = 0;

            List<FieldRegularCustomerView> RegularCustomerList =
                new List<FieldRegularCustomerView>();
            //For each Customer Id in distinct
            foreach (var booking in DistinctBookingList) 
            {
                count = 0;
                FieldRegularCustomerView RegularCustomer = new FieldRegularCustomerView();
                //For each item in the first returned list
                foreach(var item in BookingList) 
                {
                    //Check if the current item Customer id = current distinct Customer Id
                    if (booking.CustomerId == item.CustomerId) 
                    {
                        count += 1;
                    }
                }

                //if count > 3 then add new instance of regular customer
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

            //Calculate start date of 6 months ago and end date 

            var UpperMonthEndDate = DateTime.UtcNow;

            var currentMonthDays = DateTime.DaysInMonth(DateTime.Now.Month, DateTime.Now.Year);

            var Days = currentMonthDays - DateTime.UtcNow.Day;

            if (Days > 0)
                UpperMonthEndDate = DateTime.UtcNow.AddDays(Days);

            var Last6MonthDate = UpperMonthEndDate.AddMonths(-6);

            DateTime LowerMonthStartDate = new DateTime();

            var Days1 = Last6MonthDate.Day - 1;

            if(Days1 > 0) 
                LowerMonthStartDate = Last6MonthDate.AddDays(-Days1);

            //Get bookings in the last 6 months

            var Past6MonthSalesReport = await
        bookingRepo.GetBookingsForReport(FieldId, UpperMonthEndDate, LowerMonthStartDate);

            List<Booking6MonthReportView> HalfYearReportList =
                new List<Booking6MonthReportView>();

            Booking6MonthReportView HalfYearReportItem =
                new Booking6MonthReportView();

            int CurrentMonth = 0;
            int PreviousMonth = 0;

            //if there are booking in the last 6 months 
            if (Past6MonthSalesReport.Count > 0)
            {
                //for each booking add to report instance
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

            //Calculate start date of last 7 days and end date of next 7 days
            var UpperDayLimit = DateTime.UtcNow.AddDays(7);
            var LowerDayLimit = DateTime.UtcNow.AddDays(-7);
            
            //Get bookings in the last 7 days and the next 7 days
            var Past14DaysSalesReport =
                await bookingRepo.GetBookingsForReport(FieldId, UpperDayLimit, LowerDayLimit);

            List<Booking14DaysReportView> Booking14DaysReportList =
                new List<Booking14DaysReportView>();

            Booking14DaysReportView Booking14DaysReportItem = new Booking14DaysReportView();

            int CurrentDay = 0;
            int PreviousDay = 0;

            //if there are any bookings in the returned result 
            if (Past14DaysSalesReport.Count > 0) 
            {
                //then for each booking add it to report
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

            //Get bookings in the last month
            var LastMonthBooking = await bookingRepo.GetBookingsInLastMonth(FieldId);

            CustomBookingReportView LastMonthReport = new CustomBookingReportView();

            //if any bookings from returned result
            //then add them to report
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
            //Get requested soccer field then remove it
            var toDeleteSoccerField = await soccerFieldRepo.GetById(SoccerFieldId);

            if (toDeleteSoccerField == null) return GeneralResult<SoccerField>.Error(
                404, "No soccer field found with Id:" + SoccerFieldId);

            soccerFieldRepo.Delete(toDeleteSoccerField);
            await soccerFieldRepo.SaveAsync();

            return GeneralResult<SoccerField>.Success(toDeleteSoccerField);
        }

        public async Task<GeneralResult<ObjectListPagingInfo>> 
            RetrieveSoccerFieldsListForAdmin
            (PagingPayload pagingPayload, SoccerFieldPredicate filter)
        {
            //Create a new instance of predicate builder used to build query predicate
            var newPred = PredicateBuilder.New<SoccerField>(true);

            //list of navi props to include in query
            string? includeList = "user,";

            //Predicates to add to query (given that any one of them isn't null) 
            if (filter.Status != null) 
            {
                newPred = newPred.And(x => x.Status == filter.Status);
            }

            if ((filter.OpenTimeHour != null && filter.OpenTimeMinute != null) ||
                (filter.CloseTimeHour != null && filter.CloseTimeMinute != null))
            {
                TimeSpan? start = null;
                TimeSpan? end = null;

                if (filter.OpenTimeHour != null && filter.OpenTimeMinute != null)
                {
                    start = new TimeSpan
                        (filter.OpenTimeHour.Value, filter.OpenTimeMinute.Value, 0);

                }

                if (filter.CloseTimeHour != null && filter.CloseTimeMinute != null)
                {
                    end = new TimeSpan
                        (filter.CloseTimeHour.Value, filter.CloseTimeMinute.Value, 0);
                }

                if (start != null && end != null)
                {
                    newPred = newPred.And(x => start == x.OpenHour);
                    newPred = newPred.Or(x => end == x.CloseHour);
                }

                if (start != null && end == null)
                {
                    newPred = newPred.And(x => start == x.OpenHour);
                }

                if (start == null && end != null)
                {
                    newPred = newPred.And(x => end == x.CloseHour);
                }
            }

            //Create a new expression instance
            Expression<Func<SoccerField, bool>>? pred = null;

            //If the Predicate Builder contains any predicate then
            //convert it to an expression
            if (!(newPred.Body.ToString().StartsWith("True") &&
                newPred.Body.ToString().EndsWith("True")))
            {
                pred = newPred;
            }

            //Get paged list of items with sort, filters, included navi props
            var returnedSoccerFieldList = await soccerFieldRepo.GetPaginationAsync
                (pagingPayload.PageNum, pagingPayload.OrderColumn,
                pagingPayload.IsAscending, includeList,
                pred);

            if (returnedSoccerFieldList.Count == 0) return 
                    GeneralResult<ObjectListPagingInfo>.Error(
                404, "No soccer fields found");

            //Get total elements when running the query
            var TotalElement = await soccerFieldRepo.GetPagingTotalElement(pred);

            //Create new class to contain list result and paging info
            var FinalResult = new ObjectListPagingInfo();

            //Create new desired response 
            FinalResult.ObjectList = returnedSoccerFieldList.Select(x => new
            {
                x.Id,
                x.user.UserName,
                x.FieldName,
                x.Description,
                x.OpenHour,
                x.CloseHour,
                x.Address,
                x.Status
            }).ToList();

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
            RetrieveSoccerFieldsListForUser
            (PagingPayload pagingPayload, SoccerFieldPredicate filter)
        {
            //Create a new instance of predicate builder used to build query predicate
            var newPred = PredicateBuilder.New<SoccerField>(true);

            //list of navi props to include in query
            string? includeList = "user,";

            //Predicates to add to query (given that any one of them isn't null) 
            if (filter.Status != null)
            {
                newPred = newPred.And(x => x.Status == filter.Status);
            }

            if ((filter.OpenTimeHour != null && filter.OpenTimeMinute != null) ||
                (filter.CloseTimeHour != null && filter.CloseTimeMinute != null))
            {
                TimeSpan? start = null;
                TimeSpan? end = null;

                if (filter.OpenTimeHour != null && filter.OpenTimeMinute != null)
                {
                    start = new TimeSpan
                        (filter.OpenTimeHour.Value, filter.OpenTimeMinute.Value, 0);

                }

                if (filter.CloseTimeHour != null && filter.CloseTimeMinute != null)
                {
                    end = new TimeSpan
                        (filter.CloseTimeHour.Value, filter.CloseTimeMinute.Value, 0);
                }

                if (start != null && end != null)
                {
                    newPred = newPred.And(x => start == x.OpenHour);
                    newPred = newPred.Or(x => end == x.CloseHour);
                }

                if (start != null && end == null)
                {
                    newPred = newPred.And(x => start == x.OpenHour);
                }

                if (start == null && end != null)
                {
                    newPred = newPred.And(x => end == x.CloseHour);
                }
            }

            //Create a new expression instance
            Expression<Func<SoccerField, bool>>? pred = null;

            //If the Predicate Builder contains any predicate then
            //convert it to an expression
            if (!(newPred.Body.ToString().StartsWith("True") &&
                newPred.Body.ToString().EndsWith("True")))
            {
                pred = newPred;
            }

            //Get paged list of items with sort, filters, included navi props
            var returnedSoccerFieldList = await soccerFieldRepo.GetPaginationAsync
                (pagingPayload.PageNum, pagingPayload.OrderColumn,
                pagingPayload.IsAscending, includeList,
                pred);

            if (returnedSoccerFieldList.Count == 0) return
                    GeneralResult<ObjectListPagingInfo>.Error(
                404, "No soccer fields found");

            //Get total elements when running the query
            var TotalElement = await soccerFieldRepo.GetPagingTotalElement(pred);

            //Create new class to contain list result and paging info
            var FinalResult = new ObjectListPagingInfo();

            //Create new desired response 
            FinalResult.ObjectList = returnedSoccerFieldList.Select(x => new
            {
                x.Id,
                x.FieldName,
                x.OpenHour,
                x.CloseHour,
                x.Address,
            }).ToList();

            FinalResult.TotalElement = TotalElement;
            FinalResult.CurrentPage = pagingPayload.PageNum;

            //Calculate total pages based on total element
            var CheckRemain = TotalElement % 20;
            var SumPage = TotalElement / 20;
            if (CheckRemain > 0) FinalResult.TotalPage = SumPage + 1;
            else FinalResult.TotalPage = SumPage;

            return GeneralResult<ObjectListPagingInfo>.Success(FinalResult);
        }

        public async Task<GeneralResult<SoccerField>> 
            RetrieveASoccerFieldById(int SoccerFieldId)
        {
            //Get details of the requested soccer field 
            var retrievedSoccerField = await soccerFieldRepo.GetById(SoccerFieldId);

            if (retrievedSoccerField == null) return GeneralResult<SoccerField>.Error(
                404, "No soccer field found with Id:"+ SoccerFieldId);

            return GeneralResult<SoccerField>.Success(retrievedSoccerField);
        }

        public async Task<GeneralResult<SoccerField>> UpdateASoccerField(int Id,
            SoccerFieldUpdatePayload newSoccerFieldInfo)
        {
            var CheckExistSoccerField = await soccerFieldRepo
                .Get()
                .Where(x => x.FieldName == newSoccerFieldInfo.FieldName &&
                x.Address == x.Address).FirstOrDefaultAsync();

            if (CheckExistSoccerField != null) return GeneralResult<SoccerField>.Error(
                409, "Duplicate field name or address");

            //Get details of a requested soccer field for update
            var toUpdateSoccerField = await soccerFieldRepo.GetById(Id);

            if (toUpdateSoccerField == null) return GeneralResult<SoccerField>.Error(
                404, "No soccer field found with Id:" + Id);

            //Map new soccer field info to returned soccer field
            mapper.Map(newSoccerFieldInfo, toUpdateSoccerField);

            var newOpenHour = new TimeSpan
                (newSoccerFieldInfo.OpenTimeHour, newSoccerFieldInfo.OpenTimeMinute, 0);

            var newCloseHour = new TimeSpan
                (newSoccerFieldInfo.CloseTimeHour, newSoccerFieldInfo.CloseTimeMinute, 0);

            toUpdateSoccerField.OpenHour = newOpenHour;
            toUpdateSoccerField.CloseHour = newCloseHour;

            //and update it
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

            //Check duplicate booking
            var CheckBookingExist = await bookingRepo
                .CheckBookingDuplicate(StartTime.ToUniversalTime(), EndTime.ToUniversalTime());

            if (CheckBookingExist != null) return GeneralResult<AddedBookingView>
                    .Error(409, "Can not book, another booking with overlapping slots found");

            List<ZoneSlot> ZoneList = new List<ZoneSlot>();
            //for each zone slot change its status to 1 (occupied)
            foreach(var item in bookingInfo.SlotsIdList) 
            {
                var ZoneSlot = await zoneSlotRepo.GetById(item);
                ZoneSlot.Status = 1;
                ZoneList.Add(ZoneSlot);
            }

            //and update them
            zoneSlotRepo.BulkUpdate(ZoneList);
            await zoneSlotRepo.SaveAsync();

            var User = await userRepo.GetByUserName(bookingInfo.UserName);
            var ZoneType = await zoneTypeRepo.GetZoneTypeByName(bookingInfo.ZoneType);
            var Field = await soccerFieldRepo.GetFieldByFieldName(bookingInfo.FieldName);

            //Generate new DTO response
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

            //Create a new prepay payment
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

            //New added Booking DTO
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
