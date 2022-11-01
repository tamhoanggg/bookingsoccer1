using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.DTO.Zone;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.ImageFolder;
using BookingSoccers.Service.Models.Payload.Zone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.SoccerFieldInfo
{
    public interface IZoneService
    {
        Task<GeneralResult<Zone>> AddANewZone(
            ZoneCreatePayload zoneInfo);

        Task<GeneralResult<Zone>> AddNewZone(int FieldId, ZoneCreatePayload zoneInfo);

        Task<GeneralResult<List<ZoneSlot>>>AddZoneSlotsForZone(int FieldId,int ZoneId);

        Task<GeneralResult<Zone>> RetrieveAZoneById(int ZoneId);

        Task<GeneralResult<ObjectListPagingInfo>> RetrieveZonesList
            (PagingPayload pagingPayload, ZonePredicate filter);

        Task<GeneralResult<Zone>> UpdateAZone(int Id, ZoneUpdatePayload newZoneInfo);

        Task<GeneralResult<Zone>> RemoveAZone(int zoneId);

        Task<GeneralResult<List<ZoneView>>> GetFieldAvailZoneSlotsForADate
            (int FieldId, DateTime Date);
    }
}
