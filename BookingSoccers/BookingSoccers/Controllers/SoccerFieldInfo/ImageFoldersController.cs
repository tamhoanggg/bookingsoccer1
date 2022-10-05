using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Service.IService.BookingInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.ImageFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingSoccers.Controllers.SoccerFieldInfo
{
    [Route("api/image-folders")]
    [ApiController]
    [Authorize]
    public class ImageFoldersController : ControllerBase
    {
        private readonly BookingSoccersContext bookingSoccersContext;
        private readonly IImageFolderService imageFolderService;
        private readonly IMapper mapper;

        public ImageFoldersController(BookingSoccersContext bookingSoccersContext,
            IImageFolderService imageFolderService, IMapper mapper)
        {
            this.imageFolderService = imageFolderService;
            this.bookingSoccersContext = bookingSoccersContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetImageFolders()
        {

            var result = await imageFolderService.RetrieveAllImageFolders();

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }
         [Authorize(Roles ="FieldManager,Admin")]
        [HttpPost]
        public async Task<IActionResult> AddNewImageFolder(ImageFolderCreatePayload newImageFolderInfo)
        {
            var AddedImageFolder = await imageFolderService.AddANewImageFolder(newImageFolderInfo);

            if (AddedImageFolder.IsSuccess)
                return Ok(AddedImageFolder);

            Response.StatusCode = AddedImageFolder.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedImageFolder);

            return StatusCode(AddedImageFolder.StatusCode, response);
        }
         [Authorize(Roles ="FieldManager,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateABooking(int id,
            ImageFolderUpdatePayload NewImageFolderInfo)
        {

            var updatedImageFolder = await imageFolderService.UpdateAImageFolder(id, NewImageFolderInfo);

            if (updatedImageFolder.IsSuccess)
                return Ok(updatedImageFolder);

            Response.StatusCode = updatedImageFolder.StatusCode;

            var response = mapper.Map<ErrorResponse>(updatedImageFolder);

            return StatusCode(updatedImageFolder.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneSpecificImageFolder(int id)
        {
            var retrievedImageFolder = await imageFolderService.RetrieveAnImageFolderById(id);

            if (retrievedImageFolder.IsSuccess)
                return Ok(retrievedImageFolder);

            Response.StatusCode = retrievedImageFolder.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedImageFolder);

            return StatusCode(retrievedImageFolder.StatusCode, response);
        }
         [Authorize(Roles ="FieldManager,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAImageFolder(int id)
        {
            var deletedImageFolder = await imageFolderService.RemoveAImageFolder(id);

            if (deletedImageFolder.IsSuccess)
                return Ok(deletedImageFolder);

            Response.StatusCode = deletedImageFolder.StatusCode;

            var response = mapper.Map<ErrorResponse>(deletedImageFolder);

            return StatusCode(deletedImageFolder.StatusCode, response);
        }
    }
}
