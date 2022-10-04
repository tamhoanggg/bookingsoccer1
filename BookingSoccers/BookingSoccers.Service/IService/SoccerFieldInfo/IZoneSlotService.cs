using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.ImageFolder;
using BookingSoccers.Service.Models.Payload.ZoneSlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.SoccerFieldInfo
{
    public interface IZoneSlotService
    {
        Task<GeneralResult<ZoneSlot>> AddANewZoneSlot(
            ZoneSlotCreatePayload zoneSlotInfo);

        Task<GeneralResult<ZoneSlot>> RetrieveAZoneSlotById(int zoneSlotId);

        Task<GeneralResult<List<ZoneSlot>>> RetrieveAllZoneSlots();

        Task<GeneralResult<ZoneSlot>> UpdateAZoneSlot(int Id, ZoneSlotUpdatePayload newZoneSlotInfo);

        Task<GeneralResult<ZoneSlot>> RemoveAZoneSlot(int zoneSLotId);
    }
}
