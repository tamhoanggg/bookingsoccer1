using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.UserInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.ZoneSlot;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var toCreateZoneSlot = mapper.Map<ZoneSlot>(zoneSlotInfo);

            zoneSlotRepo.Create(toCreateZoneSlot);
            await zoneSlotRepo.SaveAsync();

            return GeneralResult<ZoneSlot>.Success(toCreateZoneSlot);
        }

        public async Task<GeneralResult<ZoneSlot>> RemoveAZoneSlot(int zoneSLotId)
        {
            var toDeleteZoneSlot = await zoneSlotRepo.GetById(zoneSLotId);

            if (toDeleteZoneSlot == null) return GeneralResult<ZoneSlot>.Error(
                204, "No zone slot found with Id:" + zoneSLotId);

            zoneSlotRepo.Delete(toDeleteZoneSlot);
            await zoneSlotRepo.SaveAsync();

            return GeneralResult<ZoneSlot>.Success(toDeleteZoneSlot);
        }

        public async Task<GeneralResult<List<ZoneSlot>>> RetrieveAllZoneSlots()
        {
            var ZoneSlotList = await zoneSlotRepo.Get().ToListAsync();

            if (ZoneSlotList == null) return GeneralResult<List<ZoneSlot>>.Error(
                204, "No zone slot found");

            return GeneralResult<List<ZoneSlot>>.Success(ZoneSlotList);
        }

        public async Task<GeneralResult<ZoneSlot>> RetrieveAZoneSlotById(int zoneSlotId)
        {
            var retrievedZoneSlot = await zoneSlotRepo.GetById(zoneSlotId);

            if (retrievedZoneSlot == null) return GeneralResult<ZoneSlot>.Error(
                204, "No zone slot found with Id:" + zoneSlotId);

            return GeneralResult<ZoneSlot>.Success(retrievedZoneSlot);
        }

        public async Task<GeneralResult<ZoneSlot>> UpdateAZoneSlot(int Id, ZoneSlotUpdatePayload newZoneSlotInfo)
        {
            var toUpdateZoneSlot = await zoneSlotRepo.GetById(Id);

            if (toUpdateZoneSlot == null) return GeneralResult<ZoneSlot>.Error(
                204, "No zone slot found with Id:" + Id);

            mapper.Map(newZoneSlotInfo, toUpdateZoneSlot);

            zoneSlotRepo.Update(toUpdateZoneSlot);
            await zoneSlotRepo.SaveAsync();

            return GeneralResult<ZoneSlot>.Success(toUpdateZoneSlot);
        }
    }
}
