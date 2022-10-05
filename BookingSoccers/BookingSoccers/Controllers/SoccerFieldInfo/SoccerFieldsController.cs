using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.SoccerField;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingSoccers.Controllers.SoccerFieldInfo
{
    [Route("api/soccer-fields")]
    [ApiController]
    [Authorize]
    public class SoccerFieldsController : ControllerBase
    {
        private readonly BookingSoccersContext bookingSoccersContext;
        private readonly ISoccerFieldService soccerFieldService;
        private readonly IMapper mapper;

        public SoccerFieldsController(BookingSoccersContext bookingSoccersContext,
            ISoccerFieldService soccerFieldService, IMapper mapper)
        {
            this.soccerFieldService = soccerFieldService;
            this.bookingSoccersContext = bookingSoccersContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetSoccerFields()
        {

            var result = await soccerFieldService.RetrieveAllSoccerFields();

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }
         [Authorize(Roles ="FieldManager,Admin")]
        [HttpPost]
        public async Task<IActionResult> AddNewSoccerField(SoccerFieldCreatePayload newSoccerFieldInfo)
        {
            var AddedSoccerField = await soccerFieldService.AddANewSoccerField(newSoccerFieldInfo);

            if (AddedSoccerField.IsSuccess)
                return Ok(AddedSoccerField);

            Response.StatusCode = AddedSoccerField.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedSoccerField);

            return StatusCode(AddedSoccerField.StatusCode, response);
        }
         [Authorize(Roles ="FieldManager,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateASoccerField(int id,
            SoccerFieldUpdatePayload NewSoccerFieldInfo)
        {

            var updatedSoccerField = await soccerFieldService.UpdateASoccerField(id, NewSoccerFieldInfo);

            if (updatedSoccerField.IsSuccess)
                return Ok(updatedSoccerField);

            Response.StatusCode = updatedSoccerField.StatusCode;

            var response = mapper.Map<ErrorResponse>(updatedSoccerField);

            return StatusCode(updatedSoccerField.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneSpecificSoccerField(int id)
        {
            var retrievedSoccerField = await soccerFieldService.RetrieveASoccerFieldById(id);

            if (retrievedSoccerField.IsSuccess)
                return Ok(retrievedSoccerField);

            Response.StatusCode = retrievedSoccerField.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedSoccerField);

            return StatusCode(retrievedSoccerField.StatusCode, response);
        }
         [Authorize(Roles ="FieldManager,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteASoccerField(int id)
        {
            var deletedSoccerField = await soccerFieldService.RemoveASoccerField(id);

            if (deletedSoccerField.IsSuccess)
                return Ok(deletedSoccerField);

            Response.StatusCode = deletedSoccerField.StatusCode;

            var response = mapper.Map<ErrorResponse>(deletedSoccerField);

            return StatusCode(deletedSoccerField.StatusCode, response);
        }
    }
}
