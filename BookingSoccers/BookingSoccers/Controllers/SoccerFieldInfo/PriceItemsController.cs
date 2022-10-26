using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.Payment;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.PriceItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingSoccers.Controllers.SoccerFieldInfo
{
    [Route("api/v1/price-items")]
    [ApiController]
    [Authorize]
    public class PriceItemsController : ControllerBase
    {
        private readonly BookingSoccersContext bookingSoccersContext;
        private readonly IPriceItemService priceItemService;
        private readonly IMapper mapper;

        public PriceItemsController(BookingSoccersContext bookingSoccersContext,
            IPriceItemService priceItemService, IMapper mapper)
        {
            this.priceItemService = priceItemService;
            this.bookingSoccersContext = bookingSoccersContext;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        //Get price items as list
        public async Task<IActionResult> GetPriceItemsList
            ([FromQuery] PagingPayload pagingInfo,
            [FromQuery] PriceItemPredicate predicate)
        {

            var result = await priceItemService.RetrievePriceItemsList
                (pagingInfo, predicate);

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }

        [Authorize(Roles = "Admin, FieldManager")]
        [HttpGet("{id}")]
        //Get details of a price menu's price item
        public async Task<IActionResult> GetOneSpecificPriceItem(int id)
        {
            var retrievedPriceItem = await priceItemService.RetrieveAPriceItemById(id);

            if (retrievedPriceItem.IsSuccess)
                return Ok(retrievedPriceItem);

            Response.StatusCode = retrievedPriceItem.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedPriceItem);

            return StatusCode(retrievedPriceItem.StatusCode, response);
        }

        [Authorize(Roles ="FieldManager,Admin")]
        [HttpPost]
        //Add a new price item in a price menu
        public async Task<IActionResult> AddNewPriceItem(PriceItemCreatePayload newPriceItemInfo)
        {
            var AddedPriceItem = await priceItemService.AddANewPriceItem(newPriceItemInfo);

            if (AddedPriceItem.IsSuccess)
                return Ok(AddedPriceItem);

            Response.StatusCode = AddedPriceItem.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedPriceItem);

            return StatusCode(AddedPriceItem.StatusCode, response);
        }

         [Authorize(Roles ="FieldManager,Admin")]
        [HttpPut("{id}")]
        //Update a price item of a price menu
        public async Task<IActionResult> UpdateAPriceItem(int id,
            PriceItemUpdatePayload NewPriceItemInfo)
        {

            var updatedPriceItem = await priceItemService
                .UpdateAPriceItem1(id, NewPriceItemInfo);

            if (updatedPriceItem.IsSuccess)
                return Ok(updatedPriceItem);

            Response.StatusCode = updatedPriceItem.StatusCode;

            var response = mapper.Map<ErrorResponse>(updatedPriceItem);

            return StatusCode(updatedPriceItem.StatusCode, response);
        }

         [Authorize(Roles ="FieldManager,Admin")]
        [HttpDelete("{id}")]
        //Remove a price item from a price menu
        public async Task<IActionResult> DeleteAPriceItem(int id)
        {
            var deletedPriceItem = await priceItemService.RemoveAPriceItem(id);

            if (deletedPriceItem.IsSuccess)
                return Ok(deletedPriceItem);

            Response.StatusCode = deletedPriceItem.StatusCode;

            var response = mapper.Map<ErrorResponse>(deletedPriceItem);

            return StatusCode(deletedPriceItem.StatusCode, response);
        }
    }
}
