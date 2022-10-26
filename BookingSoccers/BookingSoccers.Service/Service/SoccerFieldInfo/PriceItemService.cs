using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.BookingInfo;
using BookingSoccers.Repo.Repository.SoccerFieldInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.PriceItem;
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
    public class PriceItemService : IPriceItemService
    {
        private readonly IPriceItemRepo priceItemRepo;
        private readonly IPriceMenuRepo priceMenuRepo;
        private readonly IMapper mapper;

        public PriceItemService(IPriceItemRepo priceItemRepo, IMapper mapper,
            IPriceMenuRepo priceMenuRepo)
        {
            this.priceItemRepo = priceItemRepo;
            this.mapper = mapper;
            this.priceMenuRepo = priceMenuRepo;
        }

        public async Task<GeneralResult<PriceItem>> AddANewPriceItem
            (PriceItemCreatePayload Info)
        {
            //Get field opening hour
            var returnedPriceMenu = await priceMenuRepo
                .GetFieldOpeningHour(Info.PriceMenuId);

            var FieldOpeningHour = returnedPriceMenu.Field;

            var ItemStart = new TimeSpan(Info.StartTimeHour, Info.StartTimeMinute, 0);
            var ItemEnd = new TimeSpan(Info.EndTimeHour, Info.EndTimeMinute, 0);

            //Validate if the new price item is within field opening hour
            if (!(FieldOpeningHour.OpenHour <= ItemStart &&
                    ItemStart <= FieldOpeningHour.CloseHour) ||
                    !(FieldOpeningHour.OpenHour <= ItemEnd &&
                    ItemEnd <= FieldOpeningHour.CloseHour))
                return GeneralResult<PriceItem>.Error
                    (400, "This price item time frame is not within field opening hours");

            //Get any price items to check duplicate with the new price item
            var PriceItemExistCheck = await priceItemRepo.Get()
                .Where(x => x.PriceMenuId == Info.PriceMenuId)
                .ToListAsync();

            var FilteredCheckList = PriceItemExistCheck
                .Where(x => (ItemStart < x.StartTime &&
                x.EndTime < ItemEnd) ||
                (x.StartTime <= ItemStart && ItemStart < x.EndTime) || 
                (x.StartTime <= ItemEnd && ItemEnd < x.EndTime))
                .FirstOrDefault();

            if(FilteredCheckList != null) return
                    GeneralResult<PriceItem>.Error(409, "Price item already exists");

            //After validations, map new price item info to
            //new instance of price item and create
            var newPriceItem = mapper.Map<PriceItem>(Info);

            newPriceItem.StartTime = ItemStart;
            newPriceItem.EndTime = ItemEnd;

            priceItemRepo.Create(newPriceItem);
            await priceItemRepo.SaveAsync();

            return GeneralResult<PriceItem>.Success(newPriceItem);
        }

        public async Task<GeneralResult<PriceItem>> RemoveAPriceItem(int PriceItemId)
        {
            //Get requested price item for removing by Id then remove it
            var foundPriceItem = await priceItemRepo.GetById(PriceItemId);

            if (foundPriceItem == null) return GeneralResult<PriceItem>.Error(
                404, "No price item found with Id:" + PriceItemId);

            priceItemRepo.Delete(foundPriceItem);
            await priceItemRepo.SaveAsync();

            return GeneralResult<PriceItem>.Success(foundPriceItem);
        }

        public async Task<GeneralResult<ObjectListPagingInfo>> RetrievePriceItemsList
            (PagingPayload pagingPayload, PriceItemPredicate filter)
        {
            //Create a new instance of predicate builder used to build query predicate
            var newPred = PredicateBuilder.New<PriceItem>(true);

            //list of navi props to include in query
            string? includeList = "Menu,";

            //Predicates to add to query (given that any one of them isn't null) 
            if (filter.FromPrice != null || filter.ToPrice != null)
            {
                if (filter.FromPrice != null && filter.ToPrice != null)
                {
                    newPred = newPred.And(x => filter.FromPrice <= x.Price);
                    newPred = newPred.And(x => filter.ToPrice >= x.Price);
                }

                if (filter.FromPrice != null && filter.ToPrice == null)
                {
                    newPred = newPred.And(x => filter.FromPrice <= x.Price);
                }

                if (filter.FromPrice == null && filter.ToPrice != null)
                {
                    newPred = newPred.And(x => filter.ToPrice >= x.Price);
                }
            }

            if ((filter.StartTimeHour != null && filter.StartTimeMinute != null) ||
                (filter.EndTimeHour != null && filter.EndTimeMinute != null))
            {
                TimeSpan? start = null;
                TimeSpan? end = null;

                if (filter.StartTimeHour != null && filter.StartTimeMinute != null)
                {
                    start = new TimeSpan
                        (filter.StartTimeHour.Value, filter.StartTimeMinute.Value, 0);

                }

                if (filter.EndTimeHour != null && filter.EndTimeMinute != null)
                {
                    end = new TimeSpan
                        (filter.EndTimeHour.Value, filter.EndTimeMinute.Value, 0);
                }

                if (start != null && end != null)
                {
                    newPred = newPred.And(x => start == x.StartTime);
                    newPred = newPred.Or(x => end == x.EndTime);
                }

                if (start != null && end == null)
                {
                    newPred = newPred.And(x => start == x.StartTime);
                }

                if (start == null && end != null)
                {
                    newPred = newPred.And(x => end == x.EndTime);
                }
            }

            //Create a new expression instance
            Expression<Func<PriceItem, bool>>? pred = null;

            //If the Predicate Builder contains any predicate then
            //convert it to an expression
            if (!(newPred.Body.ToString().StartsWith("True") &&
                newPred.Body.ToString().EndsWith("True")))
            {
                pred = newPred;
            }

            //Get paged list of items with sort, filters, included navi props
            var returnedPriceItemList = await priceItemRepo.GetPaginationAsync
                (pagingPayload.PageNum, pagingPayload.OrderColumn,
                pagingPayload.IsAscending, includeList,
                 pred);

            if (returnedPriceItemList.Count == 0) return 
                GeneralResult<ObjectListPagingInfo>.Error(
                404, "No price items found");

            //Get total elements when running the query
            var TotalElement = await priceItemRepo.GetPagingTotalElement(pred);

            //Create new class to contain list result and paging info
            var FinalResult = new ObjectListPagingInfo();

            //Create new desired response 
            FinalResult.ObjectList = returnedPriceItemList.Select(x => new
            {
                x.Id,
                x.Menu.FieldId,
                DayType = x.Menu.DayType.ToString(),
                x.PriceMenuId,
                x.StartTime,
                x.EndTime,
                x.Price
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

        public async Task<GeneralResult<PriceItem>> RetrieveAPriceItemById(int priceItemId)
        {
            //Get requested price item details by Id
            var foundPriceItem = await priceItemRepo.GetById(priceItemId);

            if (foundPriceItem == null) return GeneralResult<PriceItem>.Error(
                404, "No price item found with Id:" + priceItemId);

            return GeneralResult<PriceItem>.Success(foundPriceItem);
        }

        public async Task<GeneralResult<PriceItem>> UpdateAPriceItem
            (int Id, PriceItemUpdatePayload newPriceItemInfo)
        {
            //Get requested price item details for update
            var toUpdatePriceItem = await priceItemRepo.GetById(Id);

            if (toUpdatePriceItem == null) return GeneralResult<PriceItem>.Error(
                404, "No price item found with Id:" + Id);

            //Mapping new price item info to returned price item
            mapper.Map(newPriceItemInfo, toUpdatePriceItem);

            priceItemRepo.Update(toUpdatePriceItem);
            await priceItemRepo.SaveAsync();

            return GeneralResult<PriceItem>.Success(toUpdatePriceItem);
        }

        public async Task<GeneralResult<PriceItem>> UpdateAPriceItem1
            (int Id,PriceItemUpdatePayload updateInfo)
        {
            //Get field opening hour via price menu
            var returnedPriceItem = await priceItemRepo.getFieldViaPriceItem(Id);
            var FieldOpeningHour = returnedPriceItem.Menu.Field;

            var ItemStart = new 
                TimeSpan(updateInfo.StartTimeHour, updateInfo.StartTimeMinute, 0);

            var ItemEnd = new
                TimeSpan(updateInfo.EndTimeHour, updateInfo.EndTimeMinute, 0);

            var toUpdatePriceItem = await priceItemRepo.GetById(Id);

            //If new price item start time or end time is 
            //different from that of returned price item
            if (returnedPriceItem.StartTime != ItemStart ||
               returnedPriceItem.EndTime != ItemEnd)
            {
                //Validate if the new price item start time
                //or end time is within opening hour of field
                if (!(FieldOpeningHour.OpenHour <= ItemStart &&
                    ItemStart <= FieldOpeningHour.CloseHour) ||
                    !(FieldOpeningHour.OpenHour <= ItemEnd &&
                    ItemEnd <= FieldOpeningHour.CloseHour))
                    return GeneralResult<PriceItem>.Error
                        (400, "This price item time frame is not within field opening hours");

                var ValidatePriceItem = await priceItemRepo
                    .Get()
                    .Where(x => x.Id != Id)
                    .ToListAsync();

                //Validate if there is any price item that overlaps
                //with the new price item
                var FilterList = ValidatePriceItem
                    .Where(x => (ItemStart < x.StartTime &&
                    x.EndTime < ItemEnd) ||
                    (x.StartTime <= ItemStart && ItemStart < x.EndTime) ||
                    (x.StartTime <= ItemEnd && ItemEnd < x.EndTime))
                    .ToList();

                if (FilterList.Count > 0) return GeneralResult<PriceItem>
                        .Error(400, "New price item time frame overlaps with other price items");

                toUpdatePriceItem.StartTime = ItemStart;
                toUpdatePriceItem.EndTime = ItemEnd;
            }
            
            //After successful validations, map new price item info
            //to requested price item and update
            mapper.Map(updateInfo, toUpdatePriceItem);

            priceItemRepo.Update(toUpdatePriceItem);
            await priceItemRepo.SaveAsync();

            return GeneralResult<PriceItem>.Success(toUpdatePriceItem);
        }
    }
}
