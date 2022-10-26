using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Service.Models.DTO.User;
using BookingSoccers.Service.Models.Payload.Booking;
using BookingSoccers.Service.Models.Payload.User;

namespace BookingSoccers.MapperProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserUpdatePayload>().ReverseMap();
            CreateMap<User, UserCreatePayload>().ReverseMap();
            CreateMap<User, BasicUserInfo>().ReverseMap();
            CreateMap<BasicUserInfo, UserUpdatePayload>().ReverseMap();
            CreateMap<User, UserListInfo>()
                .ForMember(x => x.RoleName,
                option => option.MapFrom(y => y.role.Name))
                .ForMember(x => x.Gender,
                option => option.MapFrom(y => y.Gender.ToString()))
                .ReverseMap();
        }
    }
}
