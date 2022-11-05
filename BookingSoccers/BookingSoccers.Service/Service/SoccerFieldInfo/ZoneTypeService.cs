using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.SoccerFieldInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.Payload.Zone;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.ZoneType;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSoccers.Repo.Repository.UserInfo;
using LinqKit;
using System.Linq.Expressions;

namespace BookingSoccers.Service.Service.SoccerFieldInfo
{
    public class ZoneTypeService : IZoneTypeService
    {
        private readonly IZoneRepo zoneRepo;
        private readonly IZoneTypeRepo zoneTypeRepo;
        private readonly IMapper mapper;

        public ZoneTypeService(IZoneTypeRepo zoneTypeRepo, IMapper mapper, IZoneRepo zoneRepo)
        {
            this.zoneTypeRepo = zoneTypeRepo;
            this.zoneRepo = zoneRepo;
            this.mapper = mapper;
        }

        public async Task<GeneralResult<ZoneType>> AddANewZoneType(ZoneTypeCreatePayload zoneTypeInfo)
        {
            //Check duplicate zone type
            var CheckZoneTypeExist = await zoneTypeRepo.
                GetZoneTypeByName(zoneTypeInfo.Name);

            if (CheckZoneTypeExist != null) return
                    GeneralResult<ZoneType>.Error(409, "Zone type already exists");

            //Then map new zone type info to new instance of zone type
            var toCreateZoneType = mapper.Map<ZoneType>(zoneTypeInfo);

            //and create
            zoneTypeRepo.Create(toCreateZoneType);
            await zoneTypeRepo.SaveAsync();

            return GeneralResult<ZoneType>.Success(toCreateZoneType);
        }

        public async Task<GeneralResult<ZoneType>> RemoveAZoneType(byte ZoneTypeId)
        {
            //Get specific zone type by Id and delete it
            var toDeleteZoneType = await zoneTypeRepo.GetById(ZoneTypeId);

            if (toDeleteZoneType == null) return GeneralResult<ZoneType>.Error(
                404, "No zone type found with Id:" + ZoneTypeId);

            zoneTypeRepo.Delete(toDeleteZoneType);
            await zoneTypeRepo.SaveAsync();

            return GeneralResult<ZoneType>.Success(toDeleteZoneType);
        }

        public async Task<GeneralResult<List<ZoneType>>> RetrieveAllZoneTypes()
        {
            //Get all zone types
            var ZoneTypeList = await zoneTypeRepo.Get().ToListAsync();

            if (ZoneTypeList.Count == 0) return GeneralResult<List<ZoneType>>.Error(
                404, "No zone type found");

            return GeneralResult<List<ZoneType>>.Success(ZoneTypeList);
        }

        public async Task<GeneralResult<ObjectListPagingInfo>> GetAZoneTypeDetails
            (PagingPayload pagingPayload, ZonePredicate filter)
        {
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
            var returnedZoneList = await zoneRepo.GetPaginationAsync
                (pagingPayload.PageNum, pagingPayload.OrderColumn,
                pagingPayload.IsAscending, includeList,
                 pred);

            if (returnedZoneList.Count == 0) return
                    GeneralResult<ObjectListPagingInfo>.Error(
                404, "No soccer field zone found");

            //Get total elements when running the query
            var TotalElement = await zoneRepo.GetPagingTotalElement(pred);

            //Create new class to contain list result and paging info
            var FinalResult = new ObjectListPagingInfo();

            //Create new desired response 
            var ZoneListInfo = returnedZoneList.Select(x => new
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

            //Get details of a zone type by Id
            var retrievedZoneSlot = await zoneTypeRepo.GetById(filter.ZoneTypeId);

            if (retrievedZoneSlot == null) return 
                    GeneralResult<ObjectListPagingInfo>.Error(
                404, "No zone type found with Id:" + filter.ZoneTypeId);

            var ZoneTypeInfo = new
            {
                retrievedZoneSlot.Id,
                retrievedZoneSlot.Name,
                ZoneListInfo
            };
            FinalResult.ObjectList = ZoneTypeInfo;

            return GeneralResult<ObjectListPagingInfo>.Success(FinalResult);
        }

        public async Task<GeneralResult<ZoneType>> UpdateAZoneType(byte Id, 
            ZoneTypeUpdatePayload newZoneTypeInfo)
        {
            //Get a specific zone type details for updating
            var toUpdateZoneType = await zoneTypeRepo.GetById(Id);

            if (toUpdateZoneType == null) return GeneralResult<ZoneType>.Error(
                404, "No zone type found with Id:" + Id);

            //Mappin new zone type info to returned zone type
            mapper.Map(newZoneTypeInfo, toUpdateZoneType);

            zoneTypeRepo.Update(toUpdateZoneType);
            await zoneTypeRepo.SaveAsync();

            return GeneralResult<ZoneType>.Success(toUpdateZoneType);
        }
    }
}
