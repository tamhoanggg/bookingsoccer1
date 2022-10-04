using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
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

        Task<GeneralResult<Zone>> RetrieveAZoneById(int ZoneId);

        Task<GeneralResult<List<Zone>>> RetrieveAllZones();

        Task<GeneralResult<Zone>> UpdateAZone(int Id, ZoneUpdatePayload newZoneInfo);

        Task<GeneralResult<Zone>> RemoveAZone(int zoneId);
    }
}
