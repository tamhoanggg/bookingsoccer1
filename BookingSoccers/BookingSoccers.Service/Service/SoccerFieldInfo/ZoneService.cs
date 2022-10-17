using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.SoccerFieldInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO.Zone;
using BookingSoccers.Service.Models.DTO.ZoneSlot;
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
        private readonly ISoccerFieldRepo soccerFieldRepo;
        private readonly IZoneSlotRepo zoneSlotRepo;
        private readonly IMapper mapper;

        public ZoneService(IZoneRepo zoneRepo, IZoneSlotRepo zoneSlotRepo,
            IMapper mapper, ISoccerFieldRepo soccerFieldRepo)
        {
            this.zoneRepo = zoneRepo;
            this.zoneSlotRepo = zoneSlotRepo;
            this.mapper = mapper;
            this.soccerFieldRepo = soccerFieldRepo;
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

        public async Task<GeneralResult<Zone>> AddNewZone(ZoneCreatePayload zoneInfo)
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

            var FieldOpeningTime = await soccerFieldRepo.GetById(zoneInfo.FieldId);

            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                DateTime.Now.Day, FieldOpeningTime.OpenHour.Hours, 
                FieldOpeningTime.OpenHour.Minutes, 0);

            TimeSpan StartTime = FieldOpeningTime.OpenHour;
            TimeSpan EndTime = FieldOpeningTime.CloseHour;

            TimeSpan ActiveHours = EndTime - StartTime;

            int Minutes = ActiveHours.Hours * 60 + ActiveHours.Minutes;
            int loopInt = Minutes / 30;

            int resetHour = (24 * 60) - Minutes;

            List<ZoneSlot> toCreateZoneSlotList = new List<ZoneSlot>();

            DateTime loopDate = date;

            for(int i =0; i <15; i++) 
            { 
                for(int a = 0; a < loopInt; a++) 
                {
                    var zoneSlot = new ZoneSlot();

                    zoneSlot.ZoneId = toCreateSoccerFieldZone.Id;
                    zoneSlot.Status = 0;
                    zoneSlot.StartTime = loopDate;

                    var NextTime = loopDate.AddMinutes(30);

                    zoneSlot.EndTime = NextTime;
                    toCreateZoneSlotList.Add(zoneSlot);

                    loopDate = NextTime;
                }
                var NextDay = loopDate.AddMinutes(resetHour);
                loopDate = NextDay;
            }

            zoneSlotRepo.BulkCreate(toCreateZoneSlotList);
            await zoneSlotRepo.SaveAsync();

            return GeneralResult<Zone>.Success(toCreateSoccerFieldZone);
        }

        public async Task<GeneralResult<List<ZoneView>>> GetFieldAvailZoneSlotsForADate
            (int FieldId, DateTime ParamDate)
        {
            var ZoneList = await zoneRepo.getFieldZonesByFieldId(FieldId);

            if (ZoneList == null) return GeneralResult<List<ZoneView>>.Error
                    (204, "No zones and zonesLot found for field id:" + FieldId);

            List<ZoneView> FinalZoneViewList = new List<ZoneView>();

            if (ZoneList != null)
            {
                foreach (var item in ZoneList)
                {
                    var SlotList = await zoneSlotRepo.getZoneSlots(item.Id, DateTime.Now);

                    ZoneView ZoneViewItem = new ZoneView();
                    ZoneViewItem.ZoneNumber = item.Number;
                    ZoneViewItem.ZoneType = item.ZoneTypeId;
                    List<ZoneSlotView> SlotViewList = new List<ZoneSlotView>();
                    foreach (var it in SlotList)
                    {
                        ZoneSlotView SlotViewItem = new ZoneSlotView()
                        {
                            SlotStartTime = it.StartTime.TimeOfDay,
                            SlotEndTime = it.EndTime.TimeOfDay
                        };

                        SlotViewList.Add(SlotViewItem);
                    }
                    ZoneViewItem.ZoneTypeSlots = SlotViewList;
                    FinalZoneViewList.Add(ZoneViewItem);
                }

                FinalZoneViewList.OrderBy(x => x.ZoneType);

            }

            return GeneralResult<List<ZoneView>>.Success(FinalZoneViewList);
            
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
