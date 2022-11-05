using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.UserInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.ZoneSlot;
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
    public class ZoneSlotService : IZoneSlotService
    {
        private readonly IZoneSlotRepo zoneSlotRepo;
        private readonly IMapper mapper;

        public ZoneSlotService(IZoneSlotRepo zoneSlotRepo, IMapper mapper)
        {
            this.zoneSlotRepo = zoneSlotRepo;
            this.mapper = mapper;
        }

        public async Task<GeneralResult<ZoneSlot>> AddANewZoneSlot(ZoneSlotCreatePayload zoneSlotInfo)
        {
            //Map new zone slot info to new instance of zone slot
            var toCreateZoneSlot = mapper.Map<ZoneSlot>(zoneSlotInfo);

            //and create
            zoneSlotRepo.Create(toCreateZoneSlot);
            await zoneSlotRepo.SaveAsync();

            return GeneralResult<ZoneSlot>.Success(toCreateZoneSlot);
        }

        public async Task<GeneralResult<ZoneSlot>> RemoveAZoneSlot(int zoneSLotId)
        {
            //Get the requested zone slot and remove it
            var toDeleteZoneSlot = await zoneSlotRepo.GetById(zoneSLotId);

            if (toDeleteZoneSlot == null) return GeneralResult<ZoneSlot>.Error(
                404, "No zone slot found with Id:" + zoneSLotId);

            zoneSlotRepo.Delete(toDeleteZoneSlot);
            await zoneSlotRepo.SaveAsync();

            return GeneralResult<ZoneSlot>.Success(toDeleteZoneSlot);
        }

        public async Task<GeneralResult<ObjectListPagingInfo>> RetrieveZoneSlotsList
            (PagingPayload pagingPayload, ZoneSlotPredicate filter)
        {
            //Create a new instance of predicate builder used to build query predicate
            var newPred = PredicateBuilder.New<ZoneSlot>(true);

            //list of navi props to include in query
            string? includeList = "FieldZone,";

            //Predicates to add to query (given that any one of them isn't null) 
            if (filter.Status != null)
            {
                newPred = newPred.And(x => x.Status == filter.Status);
            }

            if (filter.StartTimeFrom != null || filter.StartTimeTo != null)
            {
                DateTime? start = null;
                DateTime? end = null;

                if (filter.StartTimeFrom != null)
                {
                    start = filter.StartTimeFrom;

                    if (filter.StartTimeFrom.Value.Kind == DateTimeKind.Local ||
                        filter.StartTimeFrom.Value.Kind == DateTimeKind.Unspecified)
                    {
                        start.Value.ToUniversalTime();
                    }
                }

                if (filter.StartTimeTo != null)
                {
                    end = filter.StartTimeTo;

                    if (filter.StartTimeTo.Value.Kind == DateTimeKind.Local ||
                        filter.StartTimeTo.Value.Kind == DateTimeKind.Unspecified)
                    {
                        end.Value.ToUniversalTime();
                    }
                }

                if (filter.StartTimeFrom != null && filter.StartTimeTo != null)
                {
                    newPred = newPred.And(x => start <= x.StartTime);
                    newPred = newPred.And(x => end >= x.StartTime);
                }

                if (filter.StartTimeFrom != null && filter.StartTimeTo == null)
                {
                    newPred = newPred.And(x => start <= x.StartTime);
                }

                if (filter.StartTimeFrom == null && filter.StartTimeTo != null)
                {
                    newPred = newPred.And(x => end >= x.StartTime);
                }
            }

            if (filter.EndTimeFrom != null || filter.EndTimeTo != null)
            {
                DateTime? start = null;
                DateTime? end = null;

                if (filter.EndTimeFrom != null)
                {
                    start = filter.EndTimeFrom;

                    if (filter.EndTimeFrom.Value.Kind == DateTimeKind.Local ||
                        filter.EndTimeFrom.Value.Kind == DateTimeKind.Unspecified)
                    {
                        start.Value.ToUniversalTime();
                    }
                }

                if (filter.EndTimeTo != null)
                {
                    end = filter.EndTimeTo;

                    if (filter.EndTimeTo.Value.Kind == DateTimeKind.Local ||
                        filter.EndTimeTo.Value.Kind == DateTimeKind.Unspecified)
                    {
                        end.Value.ToUniversalTime();
                    }
                }

                if (filter.EndTimeFrom != null && filter.EndTimeTo != null)
                {
                    newPred = newPred.Or(x => start <= x.EndTime && end >= x.EndTime);
                }

                if (filter.EndTimeFrom != null && filter.EndTimeTo == null)
                {
                    newPred = newPred.Or(x => start <= x.EndTime);
                }

                if (filter.EndTimeFrom == null && filter.EndTimeTo != null)
                {
                    newPred = newPred.Or(x => end >= x.EndTime);
                }
            }

            //Create a new expression instance
            Expression<Func<ZoneSlot, bool>>? pred = null;

            //If the Predicate Builder contains any predicate then
            //convert it to an expression
            if (!(newPred.Body.ToString().StartsWith("True") &&
            newPred.Body.ToString().EndsWith("True")))
            {
                pred = newPred;
            }

            //Get paged list of items with sort, filters, included navi props
            var returnedZoneSlotList = await zoneSlotRepo.GetPaginationAsync
                (pagingPayload.PageNum, pagingPayload.OrderColumn,
                pagingPayload.IsAscending, includeList, pred);

            if (returnedZoneSlotList.Count == 0) return 
                    GeneralResult<ObjectListPagingInfo>.Error(
                404, "No zone slot found");

            //Get total elements when running the query
            var TotalElement = await zoneSlotRepo.GetPagingTotalElement(pred);

            //Create new class to contain list result and paging info
            var FinalResult = new ObjectListPagingInfo();

            //Create new desired response 
            FinalResult.ObjectList = returnedZoneSlotList.Select(x => new
            {
                x.Id,
                x.FieldZone.FieldId,
                ZoneNumber = x.FieldZone.Number,
                x.StartTime,
                x.EndTime,
                x.Status,
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

        public async Task<GeneralResult<Object>> GetAZoneSlotDetails(int zoneSlotId)
        {
            //Get a zone slot details by Id 
            var ZoneSlotDetails = await zoneSlotRepo.getAZoneSlotDetails(zoneSlotId);

            if (ZoneSlotDetails == null) return GeneralResult<Object>.Error(
                404, "No zone slot found with Id:" + zoneSlotId);

            var FinalResult = new
            {
                ZoneSlotDetails.Id, StartTime = ZoneSlotDetails.StartTime.ToLocalTime(),
                EndTime = ZoneSlotDetails.EndTime.ToLocalTime(), ZoneSlotDetails.Status, 
                ZoneInfo = new 
                {
                    ZoneSlotDetails.ZoneId , ZoneSlotDetails.FieldZone.ZoneTypeId, 
                    ZoneSlotDetails.FieldZone.ZoneCate.Name, 
                    ZoneSlotDetails.FieldZone.Number
                },
                FieldInfo = new 
                {
                    ZoneSlotDetails.FieldZone.FieldId, 
                    ZoneSlotDetails.FieldZone.Field.FieldName,
                    ZoneSlotDetails.FieldZone.Field.OpenHour, 
                    ZoneSlotDetails.FieldZone.Field.CloseHour, 
                    ZoneSlotDetails.FieldZone.Field.Address
                }
            };

            return GeneralResult<Object>.Success(FinalResult);
        }

        public async Task<GeneralResult<ZoneSlot>> UpdateAZoneSlot(int Id, ZoneSlotUpdatePayload newZoneSlotInfo)
        {
            //Get a specific zone slot details for updating
            var toUpdateZoneSlot = await zoneSlotRepo.GetById(Id);

            if (toUpdateZoneSlot == null) return GeneralResult<ZoneSlot>.Error(
                404, "No zone slot found with Id:" + Id);

            //Mapping new zone slot info to returned zone slot
            mapper.Map(newZoneSlotInfo, toUpdateZoneSlot);

            zoneSlotRepo.Update(toUpdateZoneSlot);
            await zoneSlotRepo.SaveAsync();

            return GeneralResult<ZoneSlot>.Success(toUpdateZoneSlot);
        }
    }
}
