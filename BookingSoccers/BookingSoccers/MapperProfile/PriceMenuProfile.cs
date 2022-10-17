using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.DTO.PriceMenu;
using BookingSoccers.Service.Models.Payload.PriceMenu;

namespace BookingSoccers.MapperProfile
{
    public class PriceMenuProfile : Profile
    {
        public PriceMenuProfile()
        {
            CreateMap<PriceMenu, PriceMenuCreatePayload>().ReverseMap();
            CreateMap<PriceMenu, PriceMenuUpdatePayload>().ReverseMap();
            CreateMap<PriceMenu, PriceMenuView>()
                .ForMember(x => x.ZoneType,
                option => option.MapFrom(y => y.TypeOfZone.Name))
                .ReverseMap();
        }
    }
}
