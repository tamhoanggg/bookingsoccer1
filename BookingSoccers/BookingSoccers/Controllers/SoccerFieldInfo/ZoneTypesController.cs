using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.Zone;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.ZoneType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingSoccers.Controllers.SoccerFieldInfo
{
    [Route("api/v1/zone-types")]
    [ApiController]
    [Authorize]
    public class ZoneTypesController : ControllerBase
    {
        private readonly BookingSoccersContext bookingSoccersContext;
        private readonly IZoneTypeService zoneTypeService;
        private readonly IMapper mapper;

        public ZoneTypesController(BookingSoccersContext bookingSoccersContext,
            IZoneTypeService zoneTypeService, IMapper mapper)
        {
            this.zoneTypeService = zoneTypeService;
            this.bookingSoccersContext = bookingSoccersContext;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        //Get all zone types 
        public async Task<IActionResult> GetZoneTypes()
        {

            var result = await zoneTypeService.RetrieveAllZoneTypes();

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        //Get details of a zone type
        public async Task<IActionResult> GetAZoneTypeDetails
            ([FromQuery]PagingPayload pagingPayload, [FromQuery]ZonePredicate filter)
        {
            var retrievedZoneType = await zoneTypeService.GetAZoneTypeDetails
                (pagingPayload, filter);

            if (retrievedZoneType.IsSuccess)
                return Ok(retrievedZoneType);

            Response.StatusCode = retrievedZoneType.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedZoneType);

            return StatusCode(retrievedZoneType.StatusCode, response);
        }

        [Authorize(Roles ="FieldManager,Admin")]
        [HttpPost]
        //Create a new zone type
        public async Task<IActionResult> AddNewZoneType(ZoneTypeCreatePayload NewZoneTypeInfo)
        {
            var AddedZoneType = await zoneTypeService.AddANewZoneType(NewZoneTypeInfo);

            if (AddedZoneType.IsSuccess)
                return Ok(AddedZoneType);

            Response.StatusCode = AddedZoneType.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedZoneType);

            return StatusCode(AddedZoneType.StatusCode, response);
        }

         [Authorize(Roles ="FieldManager,Admin")]
        [HttpPut("{id}")]
        //Update an existing zone type
        public async Task<IActionResult> UpdateAZoneType(byte id,
            ZoneTypeUpdatePayload NewZoneTypeInfo)
        {

            var updatedZoneType = await zoneTypeService.UpdateAZoneType(id, NewZoneTypeInfo);

            if (updatedZoneType.IsSuccess)
                return Ok(updatedZoneType);

            Response.StatusCode = updatedZoneType.StatusCode;

            var response = mapper.Map<ErrorResponse>(updatedZoneType);

            return StatusCode(updatedZoneType.StatusCode, response);
        }

         [Authorize(Roles ="FieldManager,Admin")]
        [HttpDelete("{id}")]
        //Delete an existing zone type
        public async Task<IActionResult> DeleteAZoneType(byte id)
        {
            var deletedZoneType = await zoneTypeService.RemoveAZoneType(id);

            if (deletedZoneType.IsSuccess)
                return Ok(deletedZoneType);

            Response.StatusCode = deletedZoneType.StatusCode;

            var response = mapper.Map<ErrorResponse>(deletedZoneType);

            return StatusCode(deletedZoneType.StatusCode, response);
        }
    }
}
