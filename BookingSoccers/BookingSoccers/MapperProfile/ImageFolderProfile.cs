using AutoMapper;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Payload.ImageFolder;
using BookingSoccers.Service.Models.Payload.PriceItem;

namespace BookingSoccers.MapperProfile
{
    public class ImageFolderProfile : Profile
    {
        public ImageFolderProfile()
        {
            CreateMap<ImageFolder, ImageFolderUpdatePayload>().ReverseMap();
            CreateMap<ImageFolder, ImageFolderCreatePayload>().ReverseMap();
        }
    }
}
