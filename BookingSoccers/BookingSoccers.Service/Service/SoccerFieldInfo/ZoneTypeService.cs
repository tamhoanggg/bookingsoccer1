using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.SoccerFieldInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.ZoneType;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Service.SoccerFieldInfo
{
    public class ZoneTypeService : IZoneTypeService
    {
        private readonly IZoneTypeRepo zoneTypeRepo;
        private readonly IMapper mapper;

        public ZoneTypeService(IZoneTypeRepo zoneTypeRepo, IMapper mapper)
        {
            this.zoneTypeRepo = zoneTypeRepo;
            this.mapper = mapper;
        }

        public async Task<GeneralResult<ZoneType>> AddANewZoneType(ZoneTypeCreatePayload zoneTypeInfo)
        {
            var CheckZoneTypeExist = await zoneTypeRepo.
                GetZoneTypeByName(zoneTypeInfo.Name);

            if (CheckZoneTypeExist != null) return
                    GeneralResult<ZoneType>.Error(403, "Zone type already exists");

            var toCreateZoneType = mapper.Map<ZoneType>(zoneTypeInfo);

            zoneTypeRepo.Create(toCreateZoneType);
            await zoneTypeRepo.SaveAsync();

            return GeneralResult<ZoneType>.Success(toCreateZoneType);
        }

        public async Task<GeneralResult<ZoneType>> RemoveAZoneType(byte ZoneTypeId)
        {
            var toDeleteZoneType = await zoneTypeRepo.GetById(ZoneTypeId);

            if (toDeleteZoneType == null) return GeneralResult<ZoneType>.Error(
                204, "No zone type found with Id:" + ZoneTypeId);

            zoneTypeRepo.Delete(toDeleteZoneType);
            await zoneTypeRepo.SaveAsync();

            return GeneralResult<ZoneType>.Success(toDeleteZoneType);
        }

        public async Task<GeneralResult<List<ZoneType>>> RetrieveAllZoneTypes()
        {
            var ZoneTypeList = await zoneTypeRepo.Get().ToListAsync();

            if (ZoneTypeList == null) return GeneralResult<List<ZoneType>>.Error(
                204, "No zone type found");

            return GeneralResult<List<ZoneType>>.Success(ZoneTypeList);
        }

        public async Task<GeneralResult<ZoneType>> RetrieveAZoneTypeById(byte zoneTypeId)
        {
            var retrievedZoneSlot = await zoneTypeRepo.GetById(zoneTypeId);

            if (retrievedZoneSlot == null) return GeneralResult<ZoneType>.Error(
                204, "No zone type found with Id:" + zoneTypeId);

            return GeneralResult<ZoneType>.Success(retrievedZoneSlot);
        }

        public async Task<GeneralResult<ZoneType>> UpdateAZoneType(byte Id, 
            ZoneTypeUpdatePayload newZoneTypeInfo)
        {
            var toUpdateZoneType = await zoneTypeRepo.GetById(Id);

            if (toUpdateZoneType == null) return GeneralResult<ZoneType>.Error(
                204, "No zone type found with Id:" + Id);

            mapper.Map(newZoneTypeInfo, toUpdateZoneType);

            zoneTypeRepo.Update(toUpdateZoneType);
            await zoneTypeRepo.SaveAsync();

            return GeneralResult<ZoneType>.Success(toUpdateZoneType);
        }
    }
}
