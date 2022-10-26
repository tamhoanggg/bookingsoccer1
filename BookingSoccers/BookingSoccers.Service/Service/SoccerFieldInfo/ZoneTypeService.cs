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
            //Check duplicate zone type
            var CheckZoneTypeExist = await zoneTypeRepo.
                GetZoneTypeByName(zoneTypeInfo.Name);

            if (CheckZoneTypeExist != null) return
                    GeneralResult<ZoneType>.Error(409, "Zone type already exists");

            //Then map new zone type info to new instance of zone type
            var toCreateZoneType = mapper.Map<ZoneType>(zoneTypeInfo);

            //and create
            zoneTypeRepo.Create(toCreateZoneType);
            await zoneTypeRepo.SaveAsync();

            return GeneralResult<ZoneType>.Success(toCreateZoneType);
        }

        public async Task<GeneralResult<ZoneType>> RemoveAZoneType(byte ZoneTypeId)
        {
            //Get specific zone type by Id and delete it
            var toDeleteZoneType = await zoneTypeRepo.GetById(ZoneTypeId);

            if (toDeleteZoneType == null) return GeneralResult<ZoneType>.Error(
                404, "No zone type found with Id:" + ZoneTypeId);

            zoneTypeRepo.Delete(toDeleteZoneType);
            await zoneTypeRepo.SaveAsync();

            return GeneralResult<ZoneType>.Success(toDeleteZoneType);
        }

        public async Task<GeneralResult<List<ZoneType>>> RetrieveAllZoneTypes()
        {
            //Get all zone types
            var ZoneTypeList = await zoneTypeRepo.Get().ToListAsync();

            if (ZoneTypeList.Count == 0) return GeneralResult<List<ZoneType>>.Error(
                404, "No zone type found");

            return GeneralResult<List<ZoneType>>.Success(ZoneTypeList);
        }

        public async Task<GeneralResult<ZoneType>> RetrieveAZoneTypeById(byte zoneTypeId)
        {
            //Get details of a zone type by Id
            var retrievedZoneSlot = await zoneTypeRepo.GetById(zoneTypeId);

            if (retrievedZoneSlot == null) return GeneralResult<ZoneType>.Error(
                404, "No zone type found with Id:" + zoneTypeId);

            return GeneralResult<ZoneType>.Success(retrievedZoneSlot);
        }

        public async Task<GeneralResult<ZoneType>> UpdateAZoneType(byte Id, 
            ZoneTypeUpdatePayload newZoneTypeInfo)
        {
            //Get a specific zone type details for updating
            var toUpdateZoneType = await zoneTypeRepo.GetById(Id);

            if (toUpdateZoneType == null) return GeneralResult<ZoneType>.Error(
                404, "No zone type found with Id:" + Id);

            //Mappin new zone type info to returned zone type
            mapper.Map(newZoneTypeInfo, toUpdateZoneType);

            zoneTypeRepo.Update(toUpdateZoneType);
            await zoneTypeRepo.SaveAsync();

            return GeneralResult<ZoneType>.Success(toUpdateZoneType);
        }
    }
}
