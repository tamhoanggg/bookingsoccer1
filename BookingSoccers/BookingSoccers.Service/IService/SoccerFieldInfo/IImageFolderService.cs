using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.ImageFolder;
using BookingSoccers.Service.Models.Payload.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.SoccerFieldInfo
{
    public interface IImageFolderService
    {
        Task<GeneralResult<ImageFolder>> AddANewImageFolder(
            ImageFolderCreatePayload imageFolderInfo);

        Task<GeneralResult<ImageFolder>> RetrieveAnImageFolderById(int imgFolderId);

        Task<GeneralResult<List<ImageFolder>>> RetrieveAllImageFolders();

        Task<GeneralResult<ImageFolder>> UpdateAImageFolder(int Id, ImageFolderUpdatePayload newImgFolderInfo);

        Task<GeneralResult<ImageFolder>> RemoveAImageFolder(int ImgFolderId);
    }
}
