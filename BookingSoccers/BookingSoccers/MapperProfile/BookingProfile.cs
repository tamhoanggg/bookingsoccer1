using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Service.Models.DTO.Booking;
using BookingSoccers.Service.Models.Payload.Booking;
using BookingSoccers.Service.Models.Payload.Payment;

namespace BookingSoccers.MapperProfile
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingUpdatePayload>().ReverseMap();
            CreateMap<Booking, BookingView>()
                .ForMember(x => x.UserName, option => option.
                MapFrom(src => src.Customer.UserName))
                .ForMember(x => x.ZoneTypeName, option => option.
                MapFrom(src => src.TypeZone.Name))
                .ForMember(x => x.ZoneNumber, option => option.
                MapFrom(src => src.ZoneInfo.Number))
                .ForMember(x => x.FieldName, option => option.
                MapFrom(src => src.FieldInfo.FieldName))
                .ForMember(x => x.FieldAddress, option => option.
                MapFrom(src => src.FieldInfo.Address))
               .ReverseMap();

            CreateMap<Booking, BookingView2>()
                .ForMember(x => x.ZoneTypeName, option => option.
                MapFrom(src => src.TypeZone.Name))
                .ForMember(x => x.ZoneNumber, option => option.
                MapFrom(src => src.ZoneInfo.Number))
                .ForMember(x => x.FieldName, option => option.
                MapFrom(src => src.FieldInfo.FieldName))
                .ReverseMap();

            CreateMap<Booking, BookingCreatePayload>().ReverseMap();
            CreateMap<Booking, BookingView1>()
                .ForMember(x => x.CustomerName,
                option => option.MapFrom(y => y.Customer.UserName))
                .ForMember(x => x.CustomerPhoneNumber,
                option => option.MapFrom(y => y.Customer.PhoneNumber))
                .ReverseMap();
        }
    }
}
