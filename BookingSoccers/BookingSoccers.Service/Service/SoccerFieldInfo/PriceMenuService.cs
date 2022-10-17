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

        public async Task<GeneralResult<PriceMenu>> AddANewPriceMenu(PriceMenuCreatePayload Info)
        {
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
                    GeneralResult<PriceMenu>.Error(403, "Price menu already exists");

            var newPriceMenu = mapper.Map<PriceMenu>(Info);

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

        public async Task<GeneralResult<PriceMenu>> UpdatePriceMenu1
            (int Id, PriceMenuUpdatePayload updateInfo)
        {
            var toUpdatePriceMenu = await priceMenuRepo.GetById(Id);

            List<PriceMenu> ValidatePriceMenu = new List<PriceMenu>();

            if (toUpdatePriceMenu.StartDate != updateInfo.StartDate ||
                toUpdatePriceMenu.EndDate != updateInfo.EndDate)
            {
                ValidatePriceMenu = await priceMenuRepo
                    .Get()
                    .Where(x => x.FieldId == updateInfo.FieldId && x.Id != Id &&
                    x.ZoneTypeId == updateInfo.ZoneTypeId && x.DayType == updateInfo.DayType)
                    .ToListAsync();

                var FilteredList = ValidatePriceMenu
                 .Where(x => (updateInfo.StartDate < x.StartDate && x.EndDate < updateInfo.EndDate) ||
                 (x.StartDate <= updateInfo.StartDate && updateInfo.StartDate <= x.EndDate) ||
                 (x.StartDate <= updateInfo.EndDate && updateInfo.EndDate <= x.EndDate)).ToList();

                if (FilteredList != null) return GeneralResult<PriceMenu>.Error
                        (400, "This price menu duration is overlapping 1 or more other price menus");
            }

            mapper.Map(updateInfo, toUpdatePriceMenu);

            priceMenuRepo.Update(toUpdatePriceMenu);
            await priceMenuRepo.SaveAsync();

            return GeneralResult<PriceMenu>.Success(toUpdatePriceMenu);
        }
    }
}
