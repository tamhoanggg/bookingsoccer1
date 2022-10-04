using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Payload.PriceItem;
using BookingSoccers.Service.Models.Payload.PriceMenu;

namespace BookingSoccers.MapperProfile
{
    public class PriceItemProfile : Profile
    {
        public PriceItemProfile()
        {
            CreateMap<PriceItem, PriceItemCreatePayload>().ReverseMap();
            CreateMap<PriceItem, PriceItemUpdatePayload>().ReverseMap();
        }
    }
}
