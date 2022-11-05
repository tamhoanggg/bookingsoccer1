using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.ImageFolder;
using BookingSoccers.Service.Models.Payload.Zone;
using BookingSoccers.Service.Models.Payload.ZoneType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.SoccerFieldInfo
{
    public interface IZoneTypeService
    {
        Task<GeneralResult<ZoneType>> AddANewZoneType(
            ZoneTypeCreatePayload zoneTypeInfo);

        Task<GeneralResult<ObjectListPagingInfo>> GetAZoneTypeDetails
            (PagingPayload pagingPayload, ZonePredicate filter);

        Task<GeneralResult<List<ZoneType>>> RetrieveAllZoneTypes();

        Task<GeneralResult<ZoneType>> UpdateAZoneType(byte Id, ZoneTypeUpdatePayload newZoneTypeInfo);

        Task<GeneralResult<ZoneType>> RemoveAZoneType(byte ZoneTypeId);
    }
}
