using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.DTO.Payment;
using BookingSoccers.Service.Models.Payload.Payment;
using BookingSoccers.Service.Models.Payload.SoccerField;

namespace BookingSoccers.MapperProfile
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentUpdatePayload>().ReverseMap();
            CreateMap<Payment, PaymentCreatePayload>().ReverseMap();
            CreateMap<Payment, PaymentView>()
                .ForMember(x => x.FirstName, option => 
                option.MapFrom(y => y.ReceiverInfo.FirstName))
                .ForMember(x => x.LastName, option =>
                option.MapFrom(y => y.ReceiverInfo.LastName))
                .ForMember(x => x.Gender, option =>
                option.MapFrom(y => y.ReceiverInfo.Gender))
                .ForMember(x => x.PhoneNumber, option =>
                option.MapFrom(y => y.ReceiverInfo.PhoneNumber))
                .ForMember(x => x.Email, option =>
                option.MapFrom(y => y.ReceiverInfo.Email))
                .ReverseMap();
        }
    }
}
