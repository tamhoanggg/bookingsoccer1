﻿using System;

using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Service.IService.BookingInfo;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.ImageFolder;
using Firebase.Auth;
using Firebase.Storage;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IoFile = System.IO.File;

namespace BookingSoccers.Controllers.SoccerFieldInfo
{
    [Route("api/v1/image-folders")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin, FieldManager")]
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

        [Authorize(Roles = "FieldManager,Admin")]
        [HttpPost("imageFiles")]
        public async Task<IActionResult> UploadImageFiles
            ([FromForm] List<IFormFile> files, [FromForm] ImageListCreateForm info)
        {

            var AddedImageFolder = await imageFolderService
                .AddANewImageFolder(files, info);

            if (AddedImageFolder.IsSuccess)
                return Ok(AddedImageFolder);

            Response.StatusCode = AddedImageFolder.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedImageFolder);

            return StatusCode(AddedImageFolder.StatusCode, response);

        }

        [Authorize(Roles = "FieldManager,Admin")]
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

        [Authorize(Roles = "FieldManager,Admin")]
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
