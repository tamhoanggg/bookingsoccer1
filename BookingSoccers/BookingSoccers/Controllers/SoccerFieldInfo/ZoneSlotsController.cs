using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.Zone;
using BookingSoccers.Service.Models.Payload.ZoneSlot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingSoccers.Controllers.SoccerFieldInfo
{
    [Route("api/v1/zone-slots")]
    [ApiController]
    [Authorize]
    public class ZoneSlotsController : ControllerBase
    {
        private readonly BookingSoccersContext bookingSoccersContext;
        private readonly IZoneSlotService zoneSlotService;
        private readonly IMapper mapper;

        public ZoneSlotsController(BookingSoccersContext bookingSoccersContext,
            IZoneSlotService zoneSlotService, IMapper mapper)
        {
            this.zoneSlotService = zoneSlotService;
            this.bookingSoccersContext = bookingSoccersContext;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        //Get zone slots of a zone that belongs to a field as list
        public async Task<IActionResult> GetZoneSlotsList
            ([FromQuery] PagingPayload pagingPayload,
             [FromQuery] ZoneSlotPredicate predicate)
        {
            var result = await zoneSlotService.RetrieveZoneSlotsList
                (pagingPayload, predicate);

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        //Get details of a zoneslot
        public async Task<IActionResult> GetAZoneSlotDetails(int id)
        {
            var retrievedZoneSlot = await zoneSlotService.GetAZoneSlotDetails(id);

            if (retrievedZoneSlot.IsSuccess)
                return Ok(retrievedZoneSlot);

            Response.StatusCode = retrievedZoneSlot.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedZoneSlot);

            return StatusCode(retrievedZoneSlot.StatusCode, response);
        }

        [Authorize(Roles ="FieldManager,Admin")]
        [HttpPost]
        //Create a new zone slot of a zone of a specific field 
        public async Task<IActionResult> AddNewZoneSlot(ZoneSlotCreatePayload NewZoneSlotInfo)
        {
            var AddedZoneSlot = await zoneSlotService.AddANewZoneSlot(NewZoneSlotInfo);

            if (AddedZoneSlot.IsSuccess)
                return Ok(AddedZoneSlot);

            Response.StatusCode = AddedZoneSlot.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedZoneSlot);

            return StatusCode(AddedZoneSlot.StatusCode, response);
        }

         [Authorize(Roles ="FieldManager,Admin")]
        [HttpPut("{id}")]
        //Update a zone slot of a field's zone
        public async Task<IActionResult> UpdateAZoneSlot(int id,
            ZoneSlotUpdatePayload NewZoneSlotInfo)
        {

            var updatedZoneSlot = await zoneSlotService.UpdateAZoneSlot(id, NewZoneSlotInfo);

            if (updatedZoneSlot.IsSuccess)
                return Ok(updatedZoneSlot);

            Response.StatusCode = updatedZoneSlot.StatusCode;

            var response = mapper.Map<ErrorResponse>(updatedZoneSlot);

            return StatusCode(updatedZoneSlot.StatusCode, response);
        }

         [Authorize(Roles ="FieldManager,Admin")]
        [HttpDelete("{id}")]
        //Remove a zoneslot of field's zone
        public async Task<IActionResult> DeleteAZoneSlot(int id)
        {
            var deletedZoneSlot = await zoneSlotService.RemoveAZoneSlot(id);

            if (deletedZoneSlot.IsSuccess)
                return Ok(deletedZoneSlot);

            Response.StatusCode = deletedZoneSlot.StatusCode;

            var response = mapper.Map<ErrorResponse>(deletedZoneSlot);

            return StatusCode(deletedZoneSlot.StatusCode, response);
        }
    }
}
