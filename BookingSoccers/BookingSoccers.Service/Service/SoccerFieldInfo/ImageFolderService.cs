using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.BookingInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.ImageFolder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Service.SoccerFieldInfo
{
    public class ImageFolderService : IImageFolderService
    {
        private readonly IImageFolderRepo imageFolderRepo;
        private readonly IMapper mapper;

        public ImageFolderService(IImageFolderRepo imageFolderRepo, IMapper mapper)
        {
            this.imageFolderRepo = imageFolderRepo;
            this.mapper = mapper;
        }

        public async Task<GeneralResult<ImageFolder>> AddANewImageFolder(ImageFolderCreatePayload imageFolderInfo)
        {
            var newImgFolder = mapper.Map<ImageFolder>(imageFolderInfo);

            imageFolderRepo.Create(newImgFolder);
            await imageFolderRepo.SaveAsync();

            return GeneralResult<ImageFolder>.Success(newImgFolder);
        }

        public async Task<GeneralResult<ImageFolder>> RemoveAImageFolder(int ImgFolderId)
        {
            var foundImgFolder = await imageFolderRepo.GetById(ImgFolderId);

            if (foundImgFolder == null) return GeneralResult<ImageFolder>.Error(
                204, "Image folder not found with Id:" + ImgFolderId);

            imageFolderRepo.Delete(foundImgFolder);
            await imageFolderRepo.SaveAsync();

            return GeneralResult<ImageFolder>.Success(foundImgFolder);
        }

        public async Task<GeneralResult<List<ImageFolder>>> RetrieveAllImageFolders()
        {
            var ImageFolderList = await imageFolderRepo.Get().ToListAsync();

            if (ImageFolderList == null) return GeneralResult<List<ImageFolder>>.Error(
                204, "No image folders found");

            return GeneralResult<List<ImageFolder>>.Success(ImageFolderList);
        }

        public async Task<GeneralResult<ImageFolder>> RetrieveAnImageFolderById(int imgFolderId)
        {
           
            var foundImgFolder = await imageFolderRepo.GetById(imgFolderId);

            if (foundImgFolder == null) return GeneralResult<ImageFolder>.Error(
                204, "Image folder not found with Id:" + imgFolderId);

            return GeneralResult<ImageFolder>.Success(foundImgFolder);
        }

        public async Task<GeneralResult<ImageFolder>> UpdateAImageFolder(int Id, ImageFolderUpdatePayload newImgFolderInfo)
        {
            var toUpdateImgFolder = await imageFolderRepo.GetById(Id);

            if (toUpdateImgFolder == null) return GeneralResult<ImageFolder>.Error(
                204, "No image folder found with Id:" + Id);

            mapper.Map(newImgFolderInfo, toUpdateImgFolder);

            imageFolderRepo.Update(toUpdateImgFolder);
            await imageFolderRepo.SaveAsync();

            return GeneralResult<ImageFolder>.Success(toUpdateImgFolder);
        }
    }
}
