using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Service.Models.Payload.Booking;
using BookingSoccers.Service.Models.Payload.Payment;

namespace BookingSoccers.MapperProfile
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingUpdatePayload>().ReverseMap();
            CreateMap<Booking, BookingCreatePayload>().ReverseMap();
        }
    }
}
