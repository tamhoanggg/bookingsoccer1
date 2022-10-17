using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.Booking;
using BookingSoccers.Service.Models.Payload.SoccerField;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetSoccerFieldsForAdmin()
        {

            var result = await soccerFieldService.RetrieveAllSoccerFields();

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }

        [Authorize(Roles = "User")]
        [HttpGet("user")]
        public async Task<IActionResult> GetSoccerFieldsForUser()
        {

            var result = await soccerFieldService.RetrieveAllSoccerFields();

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }

        [Authorize(Roles = "FieldManager")]
        [HttpGet("{id}/booking-schedule")]
        public async Task<IActionResult> GetFieldBookingScheduleOnDate(BookingSchedule info) 
        {
            var retrievedFieldSchedule =
                await soccerFieldService.GetFieldScheduleOfADateById(info.Id, info.Date);

            if (retrievedFieldSchedule.IsSuccess)
                return Ok(retrievedFieldSchedule);

            Response.StatusCode = retrievedFieldSchedule.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedFieldSchedule);

            return StatusCode(retrievedFieldSchedule.StatusCode, response);
        }

        [Authorize(Roles = "FieldManager, Admin")]
        [HttpGet("{id}")]
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
        public async Task<IActionResult> GetSoccerFieldsInfoForManager(int ManagerId) 
        {
            var retrievedSoccerFieldList = 
                await soccerFieldService.GetFieldsForManagerByManagerId(ManagerId);

            if (retrievedSoccerFieldList.IsSuccess)
                return Ok(retrievedSoccerFieldList);

            Response.StatusCode = retrievedSoccerFieldList.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedSoccerFieldList);

            return StatusCode(retrievedSoccerFieldList.StatusCode, response);
        }

        [Authorize(Roles = "FieldManager")]
        [HttpGet("{id}/price-menus")]
        public async Task<IActionResult> GetFieldPriceMenusByFieldId(int FieldId) 
        {
            var retrievedPriceMenusList =
                await soccerFieldService.GetPriceMenusForManagerByFieldId(FieldId);

            if (retrievedPriceMenusList.IsSuccess)
                return Ok(retrievedPriceMenusList);

            Response.StatusCode = retrievedPriceMenusList.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedPriceMenusList);

            return StatusCode(retrievedPriceMenusList.StatusCode, response);
        }

        [Authorize(Roles = "FieldManager")]
        [HttpGet("{id}/regular-customers")]
        public async Task<IActionResult> GetRegularGuestsByFieldId(int FieldId)
        {
            var returnedRegularGuestList =
                await soccerFieldService.GetRegularCustomerList(FieldId);

            if (returnedRegularGuestList.IsSuccess)
                return Ok(returnedRegularGuestList);

            Response.StatusCode = returnedRegularGuestList.StatusCode;

            var response = mapper.Map<ErrorResponse>(returnedRegularGuestList);

            return StatusCode(returnedRegularGuestList.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpGet("{id}/detail-view")]
        public async Task<IActionResult> GetFieldDetailsForView(int FieldId) 
        {
            var FieldResult =
                await soccerFieldService.GetFieldForUserByFieldID(FieldId);

            if (FieldResult.IsSuccess)
                return Ok(FieldResult);

            Response.StatusCode = FieldResult.StatusCode;

            var response = mapper.Map<ErrorResponse>(FieldResult);

            return StatusCode(FieldResult.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpGet("{id}/{date}/zone-slots")]
        public async Task<IActionResult> GetFieldAvailZoneSlots(SoccerFieldZoneSlots info)
        {
            var SlotListResult =
                await zoneService.GetFieldAvailZoneSlotsForADate(info.FieldId, info.Date);

            if (SlotListResult.IsSuccess)
                return Ok(SlotListResult);

            Response.StatusCode = SlotListResult.StatusCode;

            var response = mapper.Map<ErrorResponse>(SlotListResult);

            return StatusCode(SlotListResult.StatusCode, response);
        }

        [AllowAnonymous]
        [HttpGet("{id}/booking-validate")]
        public async Task<IActionResult> ValidateBookingForm(BookingValidateForm info)
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
        [HttpPost("{id}/booking-payment")]
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
