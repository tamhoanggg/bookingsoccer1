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
        private readonly IMapper mapper;

        public PriceItemService(IPriceItemRepo priceItemRepo, IMapper mapper)
        {
            this.priceItemRepo = priceItemRepo;
            this.mapper = mapper;
        }

        public async Task<GeneralResult<PriceItem>> AddANewPriceItem(PriceItemCreatePayload Info)
        {
            var PriceItemExistCheck = await priceItemRepo.Get()
                .Where(x => x.PriceMenuId == Info.PriceMenuId)
                .ToListAsync();

            var FilteredCheckList = PriceItemExistCheck
                .Where(x => (Info.StartTime < x.StartTime &&
                x.EndTime < Info.EndTime) ||
                (x.StartTime <= Info.StartTime && Info.StartTime < x.EndTime) || 
                (x.StartTime <= Info.EndTime && Info.EndTime < x.EndTime))
                .FirstOrDefault();

            if(FilteredCheckList != null) return
                    GeneralResult<PriceItem>.Error(403, "Price item already exists");

            var newPriceItem = mapper.Map<PriceItem>(Info);

            priceItemRepo.Create(newPriceItem);
            await priceItemRepo.SaveAsync();

            return GeneralResult<PriceItem>.Success(newPriceItem);
        }

        public async Task<GeneralResult<PriceItem>> RemoveAPriceItem(int PriceItemId)
        {
            var foundPriceItem = await priceItemRepo.GetById(PriceItemId);

            if (foundPriceItem == null) return GeneralResult<PriceItem>.Error(
                204, "No price item found with Id:" + PriceItemId);

            priceItemRepo.Delete(foundPriceItem);
            await priceItemRepo.SaveAsync();

            return GeneralResult<PriceItem>.Success(foundPriceItem);
        }

        public async Task<GeneralResult<List<PriceItem>>> RetrieveAllPriceItems()
        {
            var PriceItemList = await priceItemRepo.Get().ToListAsync();

            if (PriceItemList == null) return GeneralResult<List<PriceItem>>.Error(
                204, "No price items found");

            return GeneralResult<List<PriceItem>>.Success(PriceItemList);
        }

        public async Task<GeneralResult<PriceItem>> RetrieveAPriceItemById(int priceItemId)
        {
            var foundPriceItem = await priceItemRepo.GetById(priceItemId);

            if (foundPriceItem == null) return GeneralResult<PriceItem>.Error(
                204, "No price item found with Id:" + priceItemId);

            return GeneralResult<PriceItem>.Success(foundPriceItem);
        }

        public async Task<GeneralResult<PriceItem>> UpdateAPriceItem
            (int Id, PriceItemUpdatePayload newPriceItemInfo)
        {
            var toUpdatePriceItem = await priceItemRepo.GetById(Id);

            if (toUpdatePriceItem == null) return GeneralResult<PriceItem>.Error(
                204, "No price item found with Id:" + Id);

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

            if (returnedPriceItem.StartTime != updateInfo.StartTime ||
               returnedPriceItem.EndTime != updateInfo.EndTime)
            {
                if (!(FieldOpeningHour.OpenHour <= updateInfo.StartTime &&
                    updateInfo.StartTime <= FieldOpeningHour.CloseHour) ||
                    !(FieldOpeningHour.OpenHour <= updateInfo.EndTime &&
                    updateInfo.EndTime <= FieldOpeningHour.CloseHour))
                    return GeneralResult<PriceItem>.Error
                        (400, "This price item time frame is not within field opening hours");

                var ValidatePriceItem = await priceItemRepo
                    .Get()
                    .Where(x => x.Id != Id)
                    .ToListAsync();

                var FilterList = ValidatePriceItem
                    .Where(x => (updateInfo.StartTime < x.StartTime &&
                    x.EndTime < updateInfo.EndTime) ||
                    (x.StartTime <= updateInfo.StartTime && updateInfo.StartTime < x.EndTime) ||
                    (x.StartTime <= updateInfo.EndTime && updateInfo.EndTime < x.EndTime))
                    .ToList();

                if (FilterList != null) return GeneralResult<PriceItem>
                        .Error(400, "New price item time frame overlaps with other price items");

            }
            var toUpdatePriceItem = await priceItemRepo.GetById(Id);

            mapper.Map(updateInfo, toUpdatePriceItem);

            priceItemRepo.Update(toUpdatePriceItem);
            await priceItemRepo.SaveAsync();

            return GeneralResult<PriceItem>.Success(toUpdatePriceItem);
        }
    }
}
