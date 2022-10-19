using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.BookingInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.ImageFolder;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Service.SoccerFieldInfo
{
    public class ImageFolderService : IImageFolderService
    {
        private static string API_Key = "AIzaSyCxl4kbzsuDoDDJvz8In5fFQDHww97qr_s";
        private static string Bucket = "bookingsoccerfield.appspot.com";

        private readonly IImageFolderRepo imageFolderRepo;
        private readonly IMapper mapper;

        public ImageFolderService(IImageFolderRepo imageFolderRepo, IMapper mapper)
        {
            this.imageFolderRepo = imageFolderRepo;
            this.mapper = mapper;
        }

        public async Task<GeneralResult<List<ImageFolder>>> AddANewImageFolder
            (List<IFormFile> files, ImageListCreateForm imageFolderInfo)
        {

            if (files.Count > 0)
            {
                List<ImageFolder> FieldImageList = new List<ImageFolder>();

                foreach (var item in files)
                {
                    //fullPath = Path.GetFullPath(file.FileName);
                    Stream stream = item.OpenReadStream();

                    var auth = new FirebaseAuthProvider(new FirebaseConfig(API_Key));

                    var task = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.
                        FromResult(imageFolderInfo.AccessToken),
                        ThrowOnCancel = true
                    }
                    )
                    .Child("fieldImage")
                    .Child(imageFolderInfo.FieldId.ToString())
                    .Child(item.FileName)
                    .PutAsync(stream);

                    try
                    {
                        var link = await task;
                        Console.WriteLine(link);

                        var ImageFile = new ImageFolder()
                        {
                            FieldId = imageFolderInfo.FieldId,
                            Path = link
                        };

                        FieldImageList.Add(ImageFile);
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception thrown:" + ex);
                    }
                }

                imageFolderRepo.BulkCreate(FieldImageList);
                await imageFolderRepo.SaveAsync();
                return GeneralResult<List<ImageFolder>>.Success(FieldImageList);
            }
            else return GeneralResult<List<ImageFolder>>.Error(400, "Image list is empty");
        }

        public async Task<GeneralResult<ImageFolder>> RemoveAImageFolder(int ImgFolderId)
        {
            var foundImgFolder = await imageFolderRepo.GetById(ImgFolderId);

            if (foundImgFolder == null) return GeneralResult<ImageFolder>.Error(
                404, "Image folder not found with Id:" + ImgFolderId);

            imageFolderRepo.Delete(foundImgFolder);
            await imageFolderRepo.SaveAsync();

            return GeneralResult<ImageFolder>.Success(foundImgFolder);
        }

        public async Task<GeneralResult<List<ImageFolder>>> RetrieveAllImageFolders()
        {
            var ImageFolderList = await imageFolderRepo.Get().ToListAsync();

            if (ImageFolderList.Count == 0) return GeneralResult<List<ImageFolder>>.Error(
                404, "No image folders found");

            return GeneralResult<List<ImageFolder>>.Success(ImageFolderList);
        }

        public async Task<GeneralResult<ImageFolder>> RetrieveAnImageFolderById(int imgFolderId)
        {
           
            var foundImgFolder = await imageFolderRepo.GetById(imgFolderId);

            if (foundImgFolder == null) return GeneralResult<ImageFolder>.Error(
                404, "Image folder not found with Id:" + imgFolderId);

            return GeneralResult<ImageFolder>.Success(foundImgFolder);
        }

        public async Task<GeneralResult<ImageFolder>> UpdateAImageFolder(int Id, ImageFolderUpdatePayload newImgFolderInfo)
        {
            var toUpdateImgFolder = await imageFolderRepo.GetById(Id);

            if (toUpdateImgFolder == null) return GeneralResult<ImageFolder>.Error(
                404, "No image folder found with Id:" + Id);

            mapper.Map(newImgFolderInfo, toUpdateImgFolder);

            imageFolderRepo.Update(toUpdateImgFolder);
            await imageFolderRepo.SaveAsync();

            return GeneralResult<ImageFolder>.Success(toUpdateImgFolder);
        }
    }
}
