using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Service.IService.BookingInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.Booking;
using BookingSoccers.Service.Models.Payload.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingSoccers.Controllers.BookingInfo
{
    [Route("api/v1/payments")]
  
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly BookingSoccersContext bookingSoccersContext;
        private readonly IPaymentService paymentService;
        private readonly IMapper mapper;

        public PaymentsController(BookingSoccersContext bookingSoccersContext,
            IPaymentService paymentService, IMapper mapper)
        {
            this.paymentService = paymentService;
            this.bookingSoccersContext = bookingSoccersContext;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        //Get list of payments
        public async Task<IActionResult> GetPaymentsList
            ([FromQuery] PagingPayload pagingInfo,
            [FromQuery] PaymentPredicate predicate)
        {

            var result = await paymentService.RetrievePaymentsList(pagingInfo, predicate);

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }

        [HttpGet("{id}")]
        //Get a payment details
        public async Task<IActionResult> GetOneSpecificPayment(int id)
        {
            var retrievedPayment = await paymentService.GetAPaymentDetails(id);

            if (retrievedPayment.IsSuccess)
                return Ok(retrievedPayment);

            Response.StatusCode = retrievedPayment.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedPayment);

            return StatusCode(retrievedPayment.StatusCode, response);
        }

        [Authorize(Roles ="User,Admin")]
        [HttpPost]
        //Create a new payment
        public async Task<IActionResult> AddNewPayment(PaymentCreatePayload newPaymentInfo)
        {
            var AddedPayment = await paymentService.AddANewPayment(newPaymentInfo);

            if (AddedPayment.IsSuccess)
                return Ok(AddedPayment);

            Response.StatusCode = AddedPayment.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedPayment);

            return StatusCode(AddedPayment.StatusCode, response);
        }

        [Authorize(Roles ="User,Admin")]
        [HttpPut("{id}")]
        //Update an existing payment
        public async Task<IActionResult> UpdateAPayment(int id,
            PaymentUpdatePayload NewPaymentInfo)
        {


            var updatedPayment = await paymentService.UpdateAPayment(id, NewPaymentInfo);

            if (updatedPayment.IsSuccess)
                return Ok(updatedPayment);

            Response.StatusCode = updatedPayment.StatusCode;

            var response = mapper.Map<ErrorResponse>(updatedPayment);

            return StatusCode(updatedPayment.StatusCode, response);
        }

        [Authorize(Roles ="User,Admin")]
        [HttpDelete("{id}")]
        //Remove an existing payment
        public async Task<IActionResult> DeleteAPayment(int id)
        {
            var deletedPayment = await paymentService.RemoveAPayment(id);

            if (deletedPayment.IsSuccess)
                return Ok(deletedPayment);

            Response.StatusCode = deletedPayment.StatusCode;

            var response = mapper.Map<ErrorResponse>(deletedPayment);

            return StatusCode(deletedPayment.StatusCode, response);
        }
    }
}
