using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
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
        }
    }
}
