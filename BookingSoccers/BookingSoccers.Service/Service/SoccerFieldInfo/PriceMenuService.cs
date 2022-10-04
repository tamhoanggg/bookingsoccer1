using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.SoccerFieldInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.PriceMenu;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<GeneralResult<PriceMenu>> AddANewPriceMenu(PriceMenuCreatePayload priceMenuInfo)
        {
            var PriceMenuExistCheck = await priceMenuRepo.Get().Where(x =>
             x.FieldId == priceMenuInfo.FieldId &&
             x.ZoneTypeId == priceMenuInfo.ZoneTypeId &&
             x.DayType == priceMenuInfo.DayType && 
             x.StartDate == priceMenuInfo.StartDate && 
             x.EndDate == priceMenuInfo.EndDate).ToListAsync();

            if (PriceMenuExistCheck != null) return
                    GeneralResult<PriceMenu>.Error(403, "Price menu already exists");

            var newPriceMenu = mapper.Map<PriceMenu>(priceMenuInfo);

            priceMenuRepo.Create(newPriceMenu);
            await priceMenuRepo.SaveAsync();

            return GeneralResult<PriceMenu>.Success(newPriceMenu);
        }

        public async Task<GeneralResult<PriceMenu>> RemoveAPriceMenu(int PriceMenuId)
        {
            var foundPriceMenu = await priceMenuRepo.GetById(PriceMenuId);

            if (foundPriceMenu == null) return GeneralResult<PriceMenu>.Error(
                204, "No price menu found with Id:" + PriceMenuId);

            priceMenuRepo.Delete(foundPriceMenu);
            await priceMenuRepo.SaveAsync();

            return GeneralResult<PriceMenu>.Success(foundPriceMenu);
        }

        public async Task<GeneralResult<List<PriceMenu>>> RetrieveAllPriceMenus()
        {
            var PriceMenuList = await priceMenuRepo.Get().ToListAsync();

            if (PriceMenuList == null) return GeneralResult<List<PriceMenu>>.Error(
                204, "No price menus found");

            return GeneralResult<List<PriceMenu>>.Success(PriceMenuList);
        }

        public async Task<GeneralResult<PriceMenu>> RetrieveAPriceMenuById(int priceMenuId)
        {
            var foundPriceMenu = await priceMenuRepo.GetById(priceMenuId);

            if (foundPriceMenu == null) return GeneralResult<PriceMenu>.Error(
                204, "No price menu found with Id:" + priceMenuId);

            return GeneralResult<PriceMenu>.Success(foundPriceMenu);
        }

        public async Task<GeneralResult<PriceMenu>> UpdateAPriceMenu(int Id, PriceMenuUpdatePayload newPriceMenuInfo)
        {
            var toUpdatePriceMenu = await priceMenuRepo.GetById(Id);

            if (toUpdatePriceMenu == null) return GeneralResult<PriceMenu>.Error(
                204, "No price item found with Id:" + Id);

            mapper.Map(newPriceMenuInfo, toUpdatePriceMenu);

            priceMenuRepo.Update(toUpdatePriceMenu);
            await priceMenuRepo.SaveAsync();

            return GeneralResult<PriceMenu>.Success(toUpdatePriceMenu);
        }
    }
}
