using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.Zone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingSoccers.Controllers.ZoneInfo
{
    [Route("api/zones")]
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

        [HttpGet]
        public async Task<IActionResult> GetZones()
        {

            var result = await zoneService.RetrieveAllZones();

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }
         [Authorize(Roles ="FieldManager,Admin")]
        [HttpPost]
        public async Task<IActionResult> AddNewZone(ZoneCreatePayload newZoneInfo)
        {
            var AddedZone = await zoneService.AddANewZone(newZoneInfo);

            if (AddedZone.IsSuccess)
                return Ok(AddedZone);

            Response.StatusCode = AddedZone.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedZone);

            return StatusCode(AddedZone.StatusCode, response);
        }
         [Authorize(Roles ="FieldManager,Admin")]
        [HttpPut("{id}")]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneSpecificZone(int id)
        {
            var retrievedZone = await zoneService.RetrieveAZoneById(id);

            if (retrievedZone.IsSuccess)
                return Ok(retrievedZone);

            Response.StatusCode = retrievedZone.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedZone);

            return StatusCode(retrievedZone.StatusCode, response);
        }
         [Authorize(Roles ="FieldManager,Admin")]
        [HttpDelete("{id}")]
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
