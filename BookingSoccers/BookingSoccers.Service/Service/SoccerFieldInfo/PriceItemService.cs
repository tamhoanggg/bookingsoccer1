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

        public async Task<GeneralResult<PriceItem>> AddANewPriceItem(PriceItemCreatePayload priceItemInfo)
        {
            var PriceItemExistCheck = await priceItemRepo.Get().Where(x =>
             x.PriceMenuId == priceItemInfo.PriceMenuId && 
             x.StartTime == priceItemInfo.StartTime && 
             x.EndTime == priceItemInfo.EndTime).ToListAsync();

            if(PriceItemExistCheck != null) return
                    GeneralResult<PriceItem>.Error(403, "Price item already exists");

            var newPriceItem = mapper.Map<PriceItem>(priceItemInfo);

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

        public async Task<GeneralResult<PriceItem>> UpdateAPriceItem(int Id, PriceItemUpdatePayload newPriceItemInfo)
        {
            var toUpdatePriceItem = await priceItemRepo.GetById(Id);

            if (toUpdatePriceItem == null) return GeneralResult<PriceItem>.Error(
                204, "No price item found with Id:" + Id);

            mapper.Map(newPriceItemInfo, toUpdatePriceItem);

            priceItemRepo.Update(toUpdatePriceItem);
            await priceItemRepo.SaveAsync();

            return GeneralResult<PriceItem>.Success(toUpdatePriceItem);
        }
    }
}
