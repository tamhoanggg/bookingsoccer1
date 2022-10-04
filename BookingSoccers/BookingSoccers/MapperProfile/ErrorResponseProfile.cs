using AutoMapper;
using BookingSoccers.Service.Models.Common;

namespace BookingSoccers.MapperProfile
{
    public class ErrorResponseProfile : Profile
    {
        public ErrorResponseProfile() 
        {
            CreateMap(typeof(GeneralResult<>), typeof(ErrorResponse)).ReverseMap();
        }
    }
}
