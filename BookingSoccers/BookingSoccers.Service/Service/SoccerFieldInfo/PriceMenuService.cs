using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.BookingInfo;
using BookingSoccers.Repo.Repository.SoccerFieldInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.DTO.Booking;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.PriceMenu;
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
    public class PriceMenuService : IPriceMenuService
    {
        private readonly IPriceMenuRepo priceMenuRepo;
        private readonly IMapper mapper;

        public PriceMenuService(IPriceMenuRepo priceMenuRepo, IMapper mapper)
        {
            this.priceMenuRepo = priceMenuRepo;
            this.mapper = mapper;
        }

        public async Task<GeneralResult<PriceMenu>> AddANewPriceMenu(PriceMenuCreatePayload Info)
        {
            //Get price menus and check duplicate or overlaps with the new price menu
            var PriceMenuExistCheck = await priceMenuRepo.Get()
                .Where(x =>  x.FieldId == Info.FieldId && x.ZoneTypeId == Info.ZoneTypeId &&
                x.DayType == Info.DayType
                 ).ToListAsync();

            var FilteredCheckList = PriceMenuExistCheck
                .Where(x =>
             (Info.StartDate < x.StartDate && x.EndDate < Info.EndDate) ||
             (x.StartDate <= Info.StartDate && Info.StartDate <= x.EndDate) ||
             (x.StartDate <= Info.EndDate && Info.EndDate <= x.EndDate)).FirstOrDefault();

            if (FilteredCheckList != null) return
                    GeneralResult<PriceMenu>.Error(409, "Price menu already exists");

            //After validation, map new price menu info to new price menu instance
            var newPriceMenu = mapper.Map<PriceMenu>(Info);

            priceMenuRepo.Create(newPriceMenu);
            await priceMenuRepo.SaveAsync();

            return GeneralResult<PriceMenu>.Success(newPriceMenu);
        }

        public async Task<GeneralResult<PriceMenu>> RemoveAPriceMenu(int PriceMenuId)
        {
            //Get requested price menu for deletion by Id and delete it
            var foundPriceMenu = await priceMenuRepo.GetById(PriceMenuId);

            if (foundPriceMenu == null) return GeneralResult<PriceMenu>.Error(
                404, "No price menu found with Id:" + PriceMenuId);

            priceMenuRepo.Delete(foundPriceMenu);
            await priceMenuRepo.SaveAsync();

            return GeneralResult<PriceMenu>.Success(foundPriceMenu);
        }

        public async Task<GeneralResult<ObjectListPagingInfo>> RetrievePriceMenusList
            (PagingPayload pagingPayload, PriceMenuPredicate filter)
        {
            //Create a new instance of predicate builder used to build query predicate
            var newPred = PredicateBuilder.New<PriceMenu>(true);

            //list of navi props to include in query
            string? includeList = "Field,TypeOfZone";

            //Predicates to add to query (given that any one of them isn't null) 
            if (filter.ZoneTypeId != null)
            {
                newPred = newPred.And(x => x.ZoneTypeId == filter.ZoneTypeId);
            }

            if(filter.DayType != null) 
            {
                newPred = newPred.And(x => x.DayType == (DayTypeEnum)filter.DayType);
            }

            if (filter.StartDateFrom != null || filter.StartDateTo != null)
            {
                DateTime? start = null;
                DateTime? end = null;

                if (filter.StartDateFrom != null)
                {
                    start = filter.StartDateFrom;

                    if (filter.StartDateFrom.Value.Kind == DateTimeKind.Local ||
                        filter.StartDateFrom.Value.Kind == DateTimeKind.Unspecified)
                    {
                        start.Value.ToUniversalTime();
                    }
                }

                if (filter.StartDateTo != null)
                {
                    end = filter.StartDateTo;

                    if (filter.StartDateTo.Value.Kind == DateTimeKind.Local ||
                        filter.StartDateTo.Value.Kind == DateTimeKind.Unspecified)
                    {
                        end.Value.ToUniversalTime();
                    }
                }

                if (filter.StartDateFrom != null && filter.StartDateTo != null)
                {
                    newPred = newPred.And(x => start <= x.StartDate);
                    newPred = newPred.And(x => end >= x.StartDate);
                }

                if (filter.StartDateFrom != null && filter.StartDateTo == null)
                {
                    newPred = newPred.And(x => start <= x.StartDate);
                }

                if (filter.StartDateFrom == null && filter.StartDateTo != null)
                {
                    newPred = newPred.And(x => end >= x.StartDate);
                }
            }

            if (filter.EndDateFrom != null || filter.EndDateTo != null)
            {
                DateTime? start = null;
                DateTime? end = null;

                if (filter.EndDateFrom != null)
                {
                    start = filter.EndDateFrom;

                    if (filter.EndDateFrom.Value.Kind == DateTimeKind.Local ||
                        filter.EndDateFrom.Value.Kind == DateTimeKind.Unspecified)
                    {
                        start.Value.ToUniversalTime();
                    }
                }

                if (filter.EndDateTo != null)
                {
                    end = filter.EndDateTo;

                    if (filter.EndDateTo.Value.Kind == DateTimeKind.Local ||
                        filter.EndDateTo.Value.Kind == DateTimeKind.Unspecified)
                    {
                        end.Value.ToUniversalTime();
                    }
                }

                if (filter.EndDateFrom != null && filter.EndDateTo != null)
                {
                    newPred = newPred.Or(x => start <= x.EndDate && end >= x.EndDate);
                }

                if (filter.EndDateFrom != null && filter.EndDateTo == null)
                {
                    newPred = newPred.Or(x => start <= x.EndDate);
                }

                if (filter.EndDateFrom == null && filter.EndDateTo != null)
                {
                    newPred = newPred.Or(x => end >= x.EndDate);
                }
            }

            if(filter.Status != null) 
            {
                newPred = newPred.And(x => x.Status == filter.Status);
            }

            //Create a new expression instance
            Expression<Func<PriceMenu, bool>>? pred = null;

            //If the Predicate Builder contains any predicate then
            //convert it to an expression
            if (!(newPred.Body.ToString().StartsWith("True") &&
            newPred.Body.ToString().EndsWith("True")))
            {
                pred = newPred;
            }

            //Get paged list of items with sort, filters, included navi props
            var returnedPriceMenuList = await priceMenuRepo.GetPaginationAsync
                (pagingPayload.PageNum, pagingPayload.OrderColumn,
                pagingPayload.IsAscending, includeList, pred);

            if (returnedPriceMenuList.Count == 0) return 
                    GeneralResult<ObjectListPagingInfo>.Error(
                404, "No price menus found");

            //Get total elements when running the query
            var TotalElement = await priceMenuRepo.GetPagingTotalElement(pred);

            //Create new class to contain list result and paging info
            var FinalResult = new ObjectListPagingInfo();

            //Create new desired response 
            FinalResult.ObjectList = returnedPriceMenuList.Select(x => new
            {
                x.Id,  x.Field.FieldName, x.FieldId, x.TypeOfZone.Name, x.ZoneTypeId,
                DayType = x.DayType.ToString(), x.StartDate, x.EndDate, x.Status
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

        public async Task<GeneralResult<PriceMenu>> RetrieveAPriceMenuById(int priceMenuId)
        {
            //Get the requested price menu details
            var foundPriceMenu = await priceMenuRepo.GetById(priceMenuId);

            if (foundPriceMenu == null) return GeneralResult<PriceMenu>.Error(
                404, "No price menu found with Id:" + priceMenuId);

            return GeneralResult<PriceMenu>.Success(foundPriceMenu);
        }

        public async Task<GeneralResult<PriceMenu>> UpdateAPriceMenu(int Id, PriceMenuUpdatePayload newPriceMenuInfo)
        {
            //Get requested price menu details for update
            var toUpdatePriceMenu = await priceMenuRepo.GetById(Id);

            if (toUpdatePriceMenu == null) return GeneralResult<PriceMenu>.Error(
                404, "No price item found with Id:" + Id);

            //Mapping new price menu info to returned one and update
            mapper.Map(newPriceMenuInfo, toUpdatePriceMenu);

            priceMenuRepo.Update(toUpdatePriceMenu);
            await priceMenuRepo.SaveAsync();

            return GeneralResult<PriceMenu>.Success(toUpdatePriceMenu);
        }

        public async Task<GeneralResult<PriceMenu>> UpdatePriceMenu1
            (int Id, PriceMenuUpdatePayload updateInfo)
        {
            //Get requested price menu details for update
            var toUpdatePriceMenu = await priceMenuRepo.GetById(Id);

            List<PriceMenu> ValidatePriceMenu = new List<PriceMenu>();

            //If returned price menu start, end date isn't same as
            //that of the new price menu info then
            if (toUpdatePriceMenu.StartDate != updateInfo.StartDate ||
                toUpdatePriceMenu.EndDate != updateInfo.EndDate)
            {
                //Check for price menu duplicate or time overlaps
                ValidatePriceMenu = await priceMenuRepo
                    .Get()
                    .Where(x => x.FieldId == updateInfo.FieldId && x.Id != Id &&
                    x.ZoneTypeId == updateInfo.ZoneTypeId && x.DayType == updateInfo.DayType)
                    .ToListAsync();

                var FilteredList = ValidatePriceMenu
                 .Where(x => (updateInfo.StartDate < x.StartDate && x.EndDate < updateInfo.EndDate) ||
                 (x.StartDate <= updateInfo.StartDate && updateInfo.StartDate <= x.EndDate) ||
                 (x.StartDate <= updateInfo.EndDate && updateInfo.EndDate <= x.EndDate)).ToList();

                if (FilteredList.Count > 0) return GeneralResult<PriceMenu>.Error
                        (400, "This price menu duration is overlapping 1 or more other price menus");
            }

            //After successful check, map new price menu info to
            //returned price menu and update
            mapper.Map(updateInfo, toUpdatePriceMenu);

            priceMenuRepo.Update(toUpdatePriceMenu);
            await priceMenuRepo.SaveAsync();

            return GeneralResult<PriceMenu>.Success(toUpdatePriceMenu);
        }
    }
}
