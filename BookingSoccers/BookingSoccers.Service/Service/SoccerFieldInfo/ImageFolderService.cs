using AutoMapper;
using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.BookingInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using BookingSoccers.Repo.Repository.BookingInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.Payload;
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

            //if list of type iformfile has any file in it
            if (files.Count > 0)
            {
                List<ImageFolder> FieldImageList = new List<ImageFolder>();

                //then for each file:
                foreach (var item in files)
                {
                    //Read the file stream
                    Stream stream = item.OpenReadStream();

                    //Establish connection to firebase app
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(API_Key));

                    //Create a new instance of Firebase Storage with config options
                    var task = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.
                        FromResult(imageFolderInfo.AccessToken),
                        ThrowOnCancel = true
                    }
                    )
                    //then specify path for the image using Child
                    .Child("fieldImage")
                    .Child(imageFolderInfo.FieldId.ToString())
                    .Child(item.FileName)
                    //and upload to Firebase Storage using PutAsync
                    .PutAsync(stream);

                    try
                    {
                        //Get uploaded image link for display and download
                        var link = await task;
                        Console.WriteLine(link);

                        //New instance of image folder with FieldId and Path = link
                        var ImageFile = new ImageFolder()
                        {
                            FieldId = imageFolderInfo.FieldId,
                            Path = link
                        };

                        //Add created instance above to image folder list
                        FieldImageList.Add(ImageFile);
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception thrown:" + ex);
                    }
                }

                //Bulk create for each item in image folder list
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

        public async Task<GeneralResult<ObjectListPagingInfo>> RetrieveImageFoldersList
            (PagingPayload pagingPayload)
        {
            //list of navi props to include in query
            string? includeList = "Field,";

            //Get paged list of items with sort, filters, included navi props
            var returnedImageFolderList = await imageFolderRepo.GetPaginationAsync
                (pagingPayload.PageNum, pagingPayload.OrderColumn,
                pagingPayload.IsAscending, includeList,
                 null);

            if (returnedImageFolderList.Count() == 0) return
                GeneralResult<ObjectListPagingInfo>
                    .Error(404, "No image folders found ");

            //Get total elements when running the query
            var TotalElement = await imageFolderRepo.GetPagingTotalElement(null);

            //Create new class to contain list result and paging info
            var FinalResult = new ObjectListPagingInfo();

            //Create new desired response 
            FinalResult.ObjectList = returnedImageFolderList.Select(x => new
            {
                x.Id,  x.Field.FieldName, x.FieldId, x.Path
            }).ToList();

            FinalResult.TotalElement = TotalElement;
            FinalResult.CurrentPage = pagingPayload.PageNum;

            //Calculate total pages based on total element
            var CheckRemain = TotalElement % 20;
            var SumPage = TotalElement / 20;
            if (CheckRemain > 0) FinalResult.TotalPage = SumPage + 1;
            else FinalResult.TotalPage = SumPage;

            return GeneralResult<ObjectListPagingInfo>.Success(FinalResult);
        }

        public async Task<GeneralResult<ImageFolder>> RetrieveAnImageFolderById(int imgFolderId)
        {
            //Get details of requested image folder using Id  
            var foundImgFolder = await imageFolderRepo.GetById(imgFolderId);

            if (foundImgFolder == null) return GeneralResult<ImageFolder>.Error(
                404, "Image folder not found with Id:" + imgFolderId);

            return GeneralResult<ImageFolder>.Success(foundImgFolder);
        }

        public async Task<GeneralResult<ImageFolder>> UpdateAImageFolder(int Id, ImageFolderUpdatePayload newImgFolderInfo)
        {
            //Get details of requested img folder for update
            var toUpdateImgFolder = await imageFolderRepo.GetById(Id);

            if (toUpdateImgFolder == null) return GeneralResult<ImageFolder>.Error(
                404, "No image folder found with Id:" + Id);

            //Mapping new img folder info to returned Img Folder and update
            mapper.Map(newImgFolderInfo, toUpdateImgFolder);

            imageFolderRepo.Update(toUpdateImgFolder);
            await imageFolderRepo.SaveAsync();

            return GeneralResult<ImageFolder>.Success(toUpdateImgFolder);
        }
    }
}
