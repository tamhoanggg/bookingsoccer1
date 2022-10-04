using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Payload.Zone;
using BookingSoccers.Service.Models.Payload.ZoneSlot;

namespace BookingSoccers.MapperProfile
{
    public class ZoneProfile : Profile
    {
        public ZoneProfile() 
        {
            CreateMap<Zone, ZoneCreatePayload>().ReverseMap();
            CreateMap<Zone, ZoneUpdatePayload>().ReverseMap();
        }
    }
}
