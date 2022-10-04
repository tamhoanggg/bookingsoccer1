using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Payload.ZoneSlot;
using BookingSoccers.Service.Models.Payload.ZoneType;

namespace BookingSoccers.MapperProfile
{
    public class ZoneSlotProfile : Profile
    {
        public ZoneSlotProfile() 
        {
            CreateMap<ZoneSlot, ZoneSlotUpdatePayload>().ReverseMap();
            CreateMap<ZoneSlot, ZoneSlotCreatePayload>().ReverseMap();
        }
    }
}
