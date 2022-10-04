using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.SoccerFieldInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.Zone;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Service.SoccerFieldInfo
{
    public class ZoneService : IZoneService
    {
        private readonly IZoneRepo zoneRepo;
        private readonly IMapper mapper;

        public ZoneService(IZoneRepo zoneRepo, IMapper mapper)
        {
            this.zoneRepo = zoneRepo;
            this.mapper = mapper;
        }

        public async Task<GeneralResult<Zone>> AddANewZone(ZoneCreatePayload zoneInfo)
        {
            var ZoneExistCheck = await zoneRepo.Get().Where(x =>
            x.FieldId == zoneInfo.FieldId &&
            x.Number == zoneInfo.Number).ToListAsync();

            if (ZoneExistCheck != null) return
                GeneralResult<Zone>.Error(
                    403, "Field zone number already exists");

            var toCreateSoccerFieldZone = mapper.Map<Zone>(zoneInfo);

            zoneRepo.Create(toCreateSoccerFieldZone);
            await zoneRepo.SaveAsync();

            return GeneralResult<Zone>.Success(toCreateSoccerFieldZone);
        }

        public async Task<GeneralResult<Zone>> RemoveAZone(int zoneId)
        {
            var toDeleteSoccerFieldZone = await zoneRepo.GetById(zoneId);

            if (toDeleteSoccerFieldZone == null) return GeneralResult<Zone>.Error(
                204, "No soccer field zone found with Id:" + zoneId);

            zoneRepo.Delete(toDeleteSoccerFieldZone);
            await zoneRepo.SaveAsync();

            return GeneralResult<Zone>.Success(toDeleteSoccerFieldZone);
        }

        public async Task<GeneralResult<List<Zone>>> RetrieveAllZones()
        {
            var soccerFieldZoneList = await zoneRepo.Get().ToListAsync();

            if (soccerFieldZoneList == null) return GeneralResult<List<Zone>>.Error(
                204, "No soccer field zone found");

            return GeneralResult<List<Zone>>.Success(soccerFieldZoneList);
        }

        public async Task<GeneralResult<Zone>> RetrieveAZoneById(int ZoneId)
        {
            var retrievedSoccerFieldZone = await zoneRepo.GetById(ZoneId);

            if (retrievedSoccerFieldZone == null) return GeneralResult<Zone>.Error(
                204, "No soccer field found with Id:" + ZoneId);

            return GeneralResult<Zone>.Success(retrievedSoccerFieldZone);
        }

        public async Task<GeneralResult<Zone>> UpdateAZone(int Id, ZoneUpdatePayload newZoneInfo)
        {
            var toUpdateSoccerFieldZone = await zoneRepo.GetById(Id);

            if (toUpdateSoccerFieldZone == null) return GeneralResult<Zone>.Error(
                204, "No soccer field zone found with Id:" + Id);

            mapper.Map(newZoneInfo, toUpdateSoccerFieldZone);

            zoneRepo.Update(toUpdateSoccerFieldZone);
            await zoneRepo.SaveAsync();

            return GeneralResult<Zone>.Success(toUpdateSoccerFieldZone);
        }
    }
}
