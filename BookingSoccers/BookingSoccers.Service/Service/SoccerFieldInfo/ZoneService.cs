using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.SoccerFieldInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.DTO.Zone;
using BookingSoccers.Service.Models.DTO.ZoneSlot;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.Zone;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Service.SoccerFieldInfo
{
    public class ZoneService : IZoneService
    {
        private readonly IZoneRepo zoneRepo;
        private readonly ISoccerFieldRepo soccerFieldRepo;
        private readonly IZoneSlotRepo zoneSlotRepo;
        private readonly IMapper mapper;

        public ZoneService(IZoneRepo zoneRepo, IZoneSlotRepo zoneSlotRepo,
            IMapper mapper, ISoccerFieldRepo soccerFieldRepo)
        {
            this.zoneRepo = zoneRepo;
            this.zoneSlotRepo = zoneSlotRepo;
            this.mapper = mapper;
            this.soccerFieldRepo = soccerFieldRepo;
        }

        public async Task<GeneralResult<Zone>> AddANewZone(ZoneCreatePayload zoneInfo)
        {
            //Check duplicate field zone number
            var ZoneExistCheck = await zoneRepo.Get().Where(x =>
            x.FieldId == zoneInfo.FieldId &&
            x.Number == zoneInfo.Number).ToListAsync();

            if (ZoneExistCheck.Count > 0) return
                GeneralResult<Zone>.Error(
                    409, "Field zone number already exists");

            //Map new zone info to new instance of zone 
            var toCreateSoccerFieldZone = mapper.Map<Zone>(zoneInfo);

            zoneRepo.Create(toCreateSoccerFieldZone);
            await zoneRepo.SaveAsync();

            return GeneralResult<Zone>.Success(toCreateSoccerFieldZone);
        }

        public async Task<GeneralResult<Zone>> AddNewZone
            (int FieldId, ZoneCreatePayload zoneInfo)
        {
            //Check duplicate zone number
            var ZoneExistCheck = await zoneRepo.Get().Where(x =>
            x.FieldId == zoneInfo.FieldId &&
            x.Number == zoneInfo.Number).ToListAsync();

            if (ZoneExistCheck.Count > 0) return
                GeneralResult<Zone>.Error(
                    409, "Field zone number already exists");

            //Map new zone info to new instance of zone
            var toCreateSoccerFieldZone = mapper.Map<Zone>(zoneInfo);

            zoneRepo.Create(toCreateSoccerFieldZone);
            await zoneRepo.SaveAsync();

            //Get field opening time
            var FieldOpeningTime = await soccerFieldRepo.GetById(FieldId);

            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                DateTime.Now.Day, FieldOpeningTime.OpenHour.Hours, 
                FieldOpeningTime.OpenHour.Minutes, 0, DateTimeKind.Utc);

            DateTime UtcDate = date.AddHours(-7);

            TimeSpan StartTime = FieldOpeningTime.OpenHour;
            TimeSpan EndTime = FieldOpeningTime.CloseHour;

            TimeSpan ActiveHours = EndTime - StartTime;

            int Minutes = ActiveHours.Hours * 60 + ActiveHours.Minutes;
            int loopInt = Minutes / 30;

            int resetHour = (24 * 60) - Minutes;

            List<ZoneSlot> toCreateZoneSlotList = new List<ZoneSlot>();

            DateTime loopDate = UtcDate;
            //Create zone slots of zone within opening time for the next specified days
            for(int i =0; i <15; i++) 
            { 
                for(int a = 0; a < loopInt; a++) 
                {
                    var zoneSlot = new ZoneSlot();

                    zoneSlot.ZoneId = toCreateSoccerFieldZone.Id;
                    zoneSlot.Status = 0;
                    zoneSlot.StartTime = loopDate;

                    var NextTime = loopDate.AddMinutes(30);

                    zoneSlot.EndTime = NextTime;
                    toCreateZoneSlotList.Add(zoneSlot);

                    loopDate = NextTime;
                }
                var NextDay = loopDate.AddMinutes(resetHour);
                loopDate = NextDay;
            }

            zoneSlotRepo.BulkCreate(toCreateZoneSlotList);
            await zoneSlotRepo.SaveAsync();

            return GeneralResult<Zone>.Success(toCreateSoccerFieldZone);
        }

        public async Task<GeneralResult<List<ZoneView>>> GetFieldAvailZoneSlotsForADate
            (int FieldId, DateTime ParamDate)
        {
            //Get zones of field
            var ZoneList = await zoneRepo.getFieldZonesByFieldId(FieldId);

            if (ZoneList.Count == 0) return GeneralResult<List<ZoneView>>.Error
                    (404, "No zones and zonesLot found for field id:" + FieldId);

            List<ZoneView> ZoneViewList = new List<ZoneView>();
            List<ZoneView> FinalZoneViewList = new List<ZoneView>();
            //If field has any zone
            if (ZoneList.Count > 0)
            {
                //then for each zone
                foreach (var item in ZoneList)
                {
                    //get zone slot of requested date
                    var SlotList = await zoneSlotRepo.getZoneSlots(item.Id, 
                        ParamDate.ToUniversalTime());

                    ZoneView ZoneViewItem = new ZoneView();
                    ZoneViewItem.ZoneNumber = item.Number;
                    ZoneViewItem.ZoneType = item.ZoneTypeId;
                    List<ZoneSlotView> SlotViewList = new List<ZoneSlotView>();
                    //and add it to slot list
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
                    ZoneViewList.Add(ZoneViewItem);
                }

                 FinalZoneViewList = ZoneViewList.OrderBy(x => x.ZoneType).ToList();

            }

            return GeneralResult<List<ZoneView>>.Success(ZoneViewList);
            
        }

        public async Task<GeneralResult<Zone>> RemoveAZone(int zoneId)
        {
            //Get requested zone and delete it
            var toDeleteSoccerFieldZone = await zoneRepo.GetById(zoneId);

            if (toDeleteSoccerFieldZone == null) return GeneralResult<Zone>.Error(
                404, "No soccer field zone found with Id:" + zoneId);

            zoneRepo.Delete(toDeleteSoccerFieldZone);
            await zoneRepo.SaveAsync();

            return GeneralResult<Zone>.Success(toDeleteSoccerFieldZone);
        }

        public async Task<GeneralResult<ObjectListPagingInfo>> RetrieveZonesList
            (PagingPayload pagingPayload, ZonePredicate filter)
        {
            //Create a new instance of predicate builder used to build query predicate
            var newPred = PredicateBuilder.New<Zone>(true);

            //list of navi props to include in query 
            string? includeList = "Field,ZoneCate";

            //Predicates to add to query (given that any one of them isn't null)
            if (filter.ZoneTypeId != null) 
            {
                newPred = newPred.And(x => x.ZoneTypeId == filter.ZoneTypeId);
            }

            //Create a new expression instance
            Expression<Func<Zone, bool>>? pred = null;

            //If the Predicate Builder contains any predicate then
            //convert it to an expression
            if (!(newPred.Body.ToString().StartsWith("True") &&
                newPred.Body.ToString().EndsWith("True")))
            {
                pred = newPred;
            }

            //Get paged list of items with sort, filters, included navi props
            var returnedSoccerFieldZoneList = await zoneRepo.GetPaginationAsync
                (pagingPayload.PageNum, pagingPayload.OrderColumn,
                pagingPayload.IsAscending, includeList,
                 pred);

            if (returnedSoccerFieldZoneList.Count == 0) return 
                    GeneralResult<ObjectListPagingInfo>.Error(
                404, "No soccer field zone found");

            //Get total elements when running the query
            var TotalElement = await zoneRepo.GetPagingTotalElement(pred);

            //Create new class to contain list result and paging info
            var FinalResult = new ObjectListPagingInfo();

            //Create new desired response 
            FinalResult.ObjectList = returnedSoccerFieldZoneList.Select(x => new
            {
                x.Id,
                x.Field.FieldName,
                x.FieldId,
                x.ZoneCate.Name,
                x.Number
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



        public async Task<GeneralResult<Object>> GetAZoneDetails(int ZoneId)
        {
            //Get a specific zone details
            var ZoneDetails = await zoneRepo.getAZoneDetails(ZoneId);

            if (ZoneDetails == null) return GeneralResult<Object>.Error(
                404, "No soccer field found with Id:" + ZoneId);

            var FinalResult = new
            {
                ZoneDetails.Id, ZoneTypeInfo = new
                { 
                    ZoneDetails.ZoneTypeId, ZoneDetails.ZoneCate.Name
                },
                ZoneDetails.Number, FieldInfo = new 
                {
                    ZoneDetails.FieldId, ZoneDetails.Field.FieldName, 
                    ZoneDetails.Field.OpenHour, ZoneDetails.Field.CloseHour, 
                    ZoneDetails.Field.Address
                }
            };

            return GeneralResult<Object>.Success(FinalResult);
        }

        public async Task<GeneralResult<Zone>> UpdateAZone(int Id, ZoneUpdatePayload newZoneInfo)
        {
            //Get zone details for update
            var toUpdateSoccerFieldZone = await zoneRepo.GetById(Id);

            if (toUpdateSoccerFieldZone == null) return GeneralResult<Zone>.Error(
                404, "No soccer field zone found with Id:" + Id);

            //Mapping from new zone info to returned zone
            mapper.Map(newZoneInfo, toUpdateSoccerFieldZone);

            zoneRepo.Update(toUpdateSoccerFieldZone);
            await zoneRepo.SaveAsync();

            return GeneralResult<Zone>.Success(toUpdateSoccerFieldZone);
        }

        public async Task<GeneralResult<List<ZoneSlot>>> AddZoneSlotsForZone(int FieldId, int ZoneId)
        {
            var maxZoneSlot = await zoneSlotRepo.getAZoneSlotByZoneId(ZoneId);

            var FieldOpeningTime = await soccerFieldRepo.GetById(FieldId);

            var UtcDate = maxZoneSlot.EndTime.ToUniversalTime();

            TimeSpan StartTime = FieldOpeningTime.OpenHour;
            TimeSpan EndTime = FieldOpeningTime.CloseHour;

            TimeSpan ActiveHours = EndTime - StartTime;
       
            int Minutes = ActiveHours.Hours * 60 + ActiveHours.Minutes;
            int loopInt = Minutes / 30;

            int resetHour = (24 * 60) - Minutes;

            List<ZoneSlot> toCreateZoneSlotList = new List<ZoneSlot>();
            DateTime loopDate = UtcDate.AddMinutes(resetHour);

            for (int i = 0; i < 15; i++)
            {
                for (int a = 0; a < loopInt; a++)
                {
                    var zoneSlot = new ZoneSlot();

                    zoneSlot.ZoneId = ZoneId;
                    zoneSlot.Status = 0;
                    zoneSlot.StartTime = loopDate;

                    var NextTime = loopDate.AddMinutes(30);

                    zoneSlot.EndTime = NextTime;
                    toCreateZoneSlotList.Add(zoneSlot);

                    loopDate = NextTime;
                }
                var NextDay = loopDate.AddMinutes(resetHour);
                loopDate = NextDay;
            }

            zoneSlotRepo.BulkCreate(toCreateZoneSlotList);
            await zoneSlotRepo.SaveAsync();

            return GeneralResult<List<ZoneSlot>>.Success(toCreateZoneSlotList);

        



        }
    }
}
