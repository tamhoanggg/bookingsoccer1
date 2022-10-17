using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.DTO.Zone;
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
            CreateMap<Zone, ZoneView>()
                .ForMember(x => x.ZoneType , 
                options => options.MapFrom(y => y.ZoneCate.Name))
                .ReverseMap();
            CreateMap<Zone, ZoneView1>()
                .ReverseMap();
        }
    }
}
