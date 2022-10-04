using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Payload.ZoneType;

namespace BookingSoccers.MapperProfile
{
    public class ZoneTypeProfile : Profile
    {
        public ZoneTypeProfile()
        {
            CreateMap<ZoneType, ZoneTypeCreatePayload>().ReverseMap();
            CreateMap<ZoneType, ZoneTypeUpdatePayload>().ReverseMap();
        }
    }
}
