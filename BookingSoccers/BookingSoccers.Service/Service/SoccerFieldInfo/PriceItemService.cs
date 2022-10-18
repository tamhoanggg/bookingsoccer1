using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.SoccerFieldInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.PriceItem;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var returnedPriceMenu = await priceMenuRepo
                .GetFieldOpeningHour(Info.PriceMenuId);

            var FieldOpeningHour = returnedPriceMenu.Field;

            var ItemStart = new TimeSpan(Info.StartTimeHour, Info.StartTimeMinute, 0);
            var ItemEnd = new TimeSpan(Info.EndTimeHour, Info.EndTimeMinute, 0);

            if (!(FieldOpeningHour.OpenHour <= ItemStart &&
                    ItemStart <= FieldOpeningHour.CloseHour) ||
                    !(FieldOpeningHour.OpenHour <= ItemEnd &&
                    ItemEnd <= FieldOpeningHour.CloseHour))
                return GeneralResult<PriceItem>.Error
                    (400, "This price item time frame is not within field opening hours");

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


            var newPriceItem = mapper.Map<PriceItem>(Info);

            newPriceItem.StartTime = ItemStart;
            newPriceItem.EndTime = ItemEnd;

            priceItemRepo.Create(newPriceItem);
            await priceItemRepo.SaveAsync();

            return GeneralResult<PriceItem>.Success(newPriceItem);
        }

        public async Task<GeneralResult<PriceItem>> RemoveAPriceItem(int PriceItemId)
        {
            var foundPriceItem = await priceItemRepo.GetById(PriceItemId);

            if (foundPriceItem == null) return GeneralResult<PriceItem>.Error(
                404, "No price item found with Id:" + PriceItemId);

            priceItemRepo.Delete(foundPriceItem);
            await priceItemRepo.SaveAsync();

            return GeneralResult<PriceItem>.Success(foundPriceItem);
        }

        public async Task<GeneralResult<List<PriceItem>>> RetrieveAllPriceItems()
        {
            var PriceItemList = await priceItemRepo.Get().ToListAsync();

            if (PriceItemList.Count == 0) return GeneralResult<List<PriceItem>>.Error(
                404, "No price items found");

            return GeneralResult<List<PriceItem>>.Success(PriceItemList);
        }

        public async Task<GeneralResult<PriceItem>> RetrieveAPriceItemById(int priceItemId)
        {
            var foundPriceItem = await priceItemRepo.GetById(priceItemId);

            if (foundPriceItem == null) return GeneralResult<PriceItem>.Error(
                404, "No price item found with Id:" + priceItemId);

            return GeneralResult<PriceItem>.Success(foundPriceItem);
        }

        public async Task<GeneralResult<PriceItem>> UpdateAPriceItem
            (int Id, PriceItemUpdatePayload newPriceItemInfo)
        {
            var toUpdatePriceItem = await priceItemRepo.GetById(Id);

            if (toUpdatePriceItem == null) return GeneralResult<PriceItem>.Error(
                404, "No price item found with Id:" + Id);

            mapper.Map(newPriceItemInfo, toUpdatePriceItem);

            priceItemRepo.Update(toUpdatePriceItem);
            await priceItemRepo.SaveAsync();

            return GeneralResult<PriceItem>.Success(toUpdatePriceItem);
        }

        public async Task<GeneralResult<PriceItem>> UpdateAPriceItem1
            (int Id,PriceItemUpdatePayload updateInfo)
        {
            
            var returnedPriceItem = await priceItemRepo.getFieldViaPriceItem(Id);
            var FieldOpeningHour = returnedPriceItem.Menu.Field;

            var ItemStart = new 
                TimeSpan(updateInfo.StartTimeHour, updateInfo.StartTimeMinute, 0);

            var ItemEnd = new
                TimeSpan(updateInfo.EndTimeHour, updateInfo.EndTimeMinute, 0);

            var toUpdatePriceItem = await priceItemRepo.GetById(Id);

            if (returnedPriceItem.StartTime != ItemStart ||
               returnedPriceItem.EndTime != ItemEnd)
            {
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
            
            mapper.Map(updateInfo, toUpdatePriceItem);

            priceItemRepo.Update(toUpdatePriceItem);
            await priceItemRepo.SaveAsync();

            return GeneralResult<PriceItem>.Success(toUpdatePriceItem);
        }
    }
}
