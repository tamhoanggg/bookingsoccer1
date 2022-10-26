using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.Booking;
using BookingSoccers.Service.Models.Payload.SoccerField;
using BookingSoccers.Service.Models.Payload.Zone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X9;

namespace BookingSoccers.Controllers.SoccerFieldInfo
{
    [Route("api/v1/soccer-fields")]
    [ApiController]
    [Authorize]
    public class SoccerFieldsController : ControllerBase
    {
        private readonly BookingSoccersContext bookingSoccersContext;
        private readonly ISoccerFieldService soccerFieldService;
        private readonly IZoneService zoneService;
        private readonly IMapper mapper;

        public SoccerFieldsController(BookingSoccersContext bookingSoccersContext,
            ISoccerFieldService soccerFieldService, IMapper mapper, IZoneService zoneService)
        {
            this.soccerFieldService = soccerFieldService;
            this.bookingSoccersContext = bookingSoccersContext;
            this.mapper = mapper;
            this.zoneService = zoneService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        //Get soccer fields as list for admin only
        public async Task<IActionResult> GetSoccerFieldsListForAdmin
            ([FromQuery] PagingPayload pagingPayload,
            [FromQuery] SoccerFieldPredicate predicate)
        {

            var result = await soccerFieldService.RetrieveSoccerFieldsListForAdmin
                (pagingPayload, predicate);

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }

        [Authorize(Roles = "User")]
        [HttpGet("user")]
        //Get soccer fields as list for user only
        public async Task<IActionResult> GetSoccerFieldsForUser
            ([FromQuery] PagingPayload pagingPayload,
            [FromQuery] SoccerFieldPredicate predicate)
        {

            var result = await soccerFieldService.RetrieveSoccerFieldsListForUser
                (pagingPayload, predicate);

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }

        [Authorize(Roles = "FieldManager")]
        [HttpGet("{id}/booking-schedule")]
        //Get bookings of a date for field manager
        public async Task<IActionResult> GetFieldBookingScheduleOnDate
            (int id, DateTime date) 
        {
            var retrievedFieldSchedule =
                await soccerFieldService.GetFieldScheduleOfADateById(id, date);

            if (retrievedFieldSchedule.IsSuccess)
                return Ok(retrievedFieldSchedule);

            Response.StatusCode = retrievedFieldSchedule.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedFieldSchedule);

            return StatusCode(retrievedFieldSchedule.StatusCode, response);
        }

        [Authorize(Roles = "FieldManager, Admin")]
        [HttpGet("{id}")]
        //Get a soccer field details for Field Manager and Admin
        public async Task<IActionResult> GetASoccerField(int id)
        {
            var retrievedSoccerField = 
                await soccerFieldService.RetrieveASoccerFieldById(id);

            if (retrievedSoccerField.IsSuccess)
                return Ok(retrievedSoccerField);

            Response.StatusCode = retrievedSoccerField.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedSoccerField);

            return StatusCode(retrievedSoccerField.StatusCode, response);
        }

        [Authorize(Roles = "FieldManager")]
        [HttpGet("field-manager/{id}")]
        //Get list of fields and theirs details owned by a field manager
        public async Task<IActionResult> GetSoccerFieldsInfoForManager(int id) 
        {
            var retrievedSoccerFieldList = 
                await soccerFieldService.GetFieldsForManagerByManagerId(id);

            if (retrievedSoccerFieldList.IsSuccess)
                return Ok(retrievedSoccerFieldList);

            Response.StatusCode = retrievedSoccerFieldList.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedSoccerFieldList);

            return StatusCode(retrievedSoccerFieldList.StatusCode, response);
        }

        [Authorize(Roles = "FieldManager")]
        [HttpGet("{id}/price-menus")]
        //Get price menus and price items of a field
        public async Task<IActionResult> GetFieldPriceMenusByFieldId(int id) 
        {
            var retrievedPriceMenusList =
                await soccerFieldService.GetPriceMenusForManagerByFieldId(id);

            if (retrievedPriceMenusList.IsSuccess)
                return Ok(retrievedPriceMenusList);

            Response.StatusCode = retrievedPriceMenusList.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedPriceMenusList);

            return StatusCode(retrievedPriceMenusList.StatusCode, response);
        }

        [Authorize(Roles = "FieldManager")]
        [HttpGet("{id}/regular-customers")]
        //Get regular customer list of a field
        public async Task<IActionResult> GetRegularGuestsByFieldId(int id)
        {
            var returnedRegularGuestList =
                await soccerFieldService.GetRegularCustomerList(id);

            if (returnedRegularGuestList.IsSuccess)
                return Ok(returnedRegularGuestList);

            Response.StatusCode = returnedRegularGuestList.StatusCode;

            var response = mapper.Map<ErrorResponse>(returnedRegularGuestList);

            return StatusCode(returnedRegularGuestList.StatusCode, response);
        }

        [Authorize(Roles = "User")]
        [HttpGet("{id}/detail-view")]
        //Get a field details view for user
        public async Task<IActionResult> GetFieldDetailsForView(int id) 
        {
            var FieldResult =
                await soccerFieldService.GetFieldForUserByFieldID(id);

            if (FieldResult.IsSuccess)
                return Ok(FieldResult);

            Response.StatusCode = FieldResult.StatusCode;

            var response = mapper.Map<ErrorResponse>(FieldResult);

            return StatusCode(FieldResult.StatusCode, response);
        }

        [Authorize(Roles = "User")]
        [HttpGet("{id}/zone-slots-by-date")]
        //Get list of zone slots of a field for user view 
        public async Task<IActionResult> GetFieldAvailZoneSlots
            (int id, SoccerFieldZoneSlots info)
        {
            var SlotListResult =
                await zoneService.GetFieldAvailZoneSlotsForADate
                (id, info.Date);

            if (SlotListResult.IsSuccess)
                return Ok(SlotListResult);

            Response.StatusCode = SlotListResult.StatusCode;

            var response = mapper.Map<ErrorResponse>(SlotListResult);

            return StatusCode(SlotListResult.StatusCode, response);
        }

        [Authorize(Roles = "User")]
        [HttpGet("booking-validate")]
        //Check availability of zone slots and pre-calculate booking price 
        //based on user booking request
        public async Task<IActionResult> ValidateBookingForm
            (BookingValidateForm info)
        {
            var Result =
                await soccerFieldService.CheckZonesAndCalculatePrice(info);

            if (Result.IsSuccess)
                return Ok(Result);

            Response.StatusCode = Result.StatusCode;

            var response = mapper.Map<ErrorResponse>(Result);

            return StatusCode(Result.StatusCode, response);
        }

        [Authorize(Roles = "User")]
        [HttpPost("booking-payment")]
        //Add a new booking and a payment of type prepay after user
        //has successfully booked
        public async Task<IActionResult> AddANewBookingAndPayment
            (BookingCreateForm newBookingInfo)
        {
            var AddedBooking = await soccerFieldService.AddANewBooking(newBookingInfo);

            if (AddedBooking.IsSuccess)
                return Ok(AddedBooking);

            Response.StatusCode = AddedBooking.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedBooking);

            return StatusCode(AddedBooking.StatusCode, response);
        }

        [Authorize(Roles ="FieldManager,Admin")]
        [HttpPost]
        //Create a new soccer field
        public async Task<IActionResult> AddNewSoccerField
            (SoccerFieldCreatePayload newSoccerFieldInfo)
        {
            var AddedSoccerField = await soccerFieldService.AddANewSoccerField(newSoccerFieldInfo);

            if (AddedSoccerField.IsSuccess)
                return Ok(AddedSoccerField);

            Response.StatusCode = AddedSoccerField.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedSoccerField);

            return StatusCode(AddedSoccerField.StatusCode, response);
        }

        [Authorize(Roles = "FieldManager")]
        [HttpPost("{id}/zone")]
        //Add to an existing field a new zone and its zone slots 
        public async Task<IActionResult> AddNewSoccerFieldZone
            (int id, ZoneCreatePayload Info)
        {
            var AddedZone = await zoneService.AddNewZone(id,Info);

            if (AddedZone.IsSuccess)
                return Ok(AddedZone);

            Response.StatusCode = AddedZone.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedZone);

            return StatusCode(AddedZone.StatusCode, response);
        }

        [Authorize(Roles ="FieldManager,Admin")]
        [HttpPut("{id}")]
        //Update an existing soccer field
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

         [Authorize(Roles ="FieldManager,Admin")]
        [HttpDelete("{id}")]
        //Remove an existing soccer field
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
