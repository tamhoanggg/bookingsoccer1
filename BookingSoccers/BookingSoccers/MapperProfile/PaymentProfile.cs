using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
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
        }
    }
}
