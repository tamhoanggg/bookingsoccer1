using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.DTO.SoccerField;
using BookingSoccers.Service.Models.Payload.ImageFolder;
using BookingSoccers.Service.Models.Payload.SoccerField;

namespace BookingSoccers.MapperProfile
{
    public class SoccerFieldProfile: Profile
    {
        public SoccerFieldProfile()
        {
            CreateMap<SoccerField, SoccerFieldUpdatePayload>().ReverseMap();
            CreateMap<SoccerField, SoccerFieldCreatePayload>().ReverseMap();
            CreateMap<SoccerField, SoccerFieldView1>()
                .ForMember(x => x.ImagePath,
                option => option.MapFrom( y => y.ImageFolder.Path))
                .ReverseMap();
            CreateMap<SoccerField, SoccerFieldListView>()
                .ForMember(x => x.ManagerPhoneNumber, 
                option => option.MapFrom(y => y.user.PhoneNumber))
                .ReverseMap();
            CreateMap<SoccerField, SoccerFieldView3>()
                .ForMember(x => x.ContactNumber,
                option => option.MapFrom(y => y.user.PhoneNumber))
                .ForMember(x => x.ImageFolderPath,
                option => option.MapFrom(y => y.ImageFolder.Path))
                .ReverseMap();
        }
    }
}
