using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.Zone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingSoccers.Controllers.ZoneInfo
{
    [Route("api/v1/zones")]
    [ApiController]
    [Authorize]
    public class ZonesController : ControllerBase
    {
        private readonly BookingSoccersContext bookingSoccersContext;
        private readonly IZoneService zoneService;
        private readonly IMapper mapper;

        public ZonesController(BookingSoccersContext bookingSoccersContext,
            IZoneService zoneService, IMapper mapper)
        {
            this.zoneService = zoneService;
            this.bookingSoccersContext = bookingSoccersContext;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        //Get zones as list
        public async Task<IActionResult> GetZonesList
            ([FromQuery] PagingPayload pagingPayload,
             [FromQuery] ZonePredicate predicate)
        {
            var result = await zoneService.RetrieveZonesList(pagingPayload, predicate);

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }

        [HttpGet("{id}")]
        //Get details of a zone
        public async Task<IActionResult> GetAZoneDetails(int id)
        {
            var retrievedZone = await zoneService.GetAZoneDetails(id);

            if (retrievedZone.IsSuccess)
                return Ok(retrievedZone);

            Response.StatusCode = retrievedZone.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedZone);

            return StatusCode(retrievedZone.StatusCode, response);
        }

        [Authorize(Roles ="FieldManager,Admin")]
        [HttpPost]
        //Add a new zone to an existing field
        public async Task<IActionResult> AddNewZone(ZoneCreatePayload newZoneInfo)
        {
            var AddedZone = await zoneService.AddANewZone(newZoneInfo);

            if (AddedZone.IsSuccess)
                return Ok(AddedZone);

            Response.StatusCode = AddedZone.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedZone);

            return StatusCode(AddedZone.StatusCode, response);
        }

        [Authorize(Roles = "FieldManager,Admin")]
        [HttpPost("{id}")]
        //Add new zone slots for an existing zone 
        public async Task<IActionResult> AddZoneSlotsForZone(int id, int fieldId)
        {
            var AddedZone = await zoneService.AddZoneSlotsForZone(fieldId,id);

            if (AddedZone.IsSuccess)
                return Ok(AddedZone);

            Response.StatusCode = AddedZone.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedZone);

            return StatusCode(AddedZone.StatusCode, response);
        }

        [Authorize(Roles ="FieldManager,Admin")]
        [HttpPut("{id}")]
        //Update an existing zone of a field
        public async Task<IActionResult> UpdateAZone(int id,
            ZoneUpdatePayload NewZoneInfo)
        {

            var updatedZone = await zoneService.UpdateAZone(id, NewZoneInfo);

            if (updatedZone.IsSuccess)
                return Ok(updatedZone);

            Response.StatusCode = updatedZone.StatusCode;

            var response = mapper.Map<ErrorResponse>(updatedZone);

            return StatusCode(updatedZone.StatusCode, response);
        }

         [Authorize(Roles ="FieldManager,Admin")]
        [HttpDelete("{id}")]
        //Remove a zone of a specific field
        public async Task<IActionResult> DeleteAZone(int id)
        {
            var deletedZone = await zoneService.RemoveAZone(id);

            if (deletedZone.IsSuccess)
                return Ok(deletedZone);

            Response.StatusCode = deletedZone.StatusCode;

            var response = mapper.Map<ErrorResponse>(deletedZone);

            return StatusCode(deletedZone.StatusCode, response);
        }
    }
}
