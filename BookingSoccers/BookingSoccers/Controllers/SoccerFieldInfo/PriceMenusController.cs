using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.PriceMenu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingSoccers.Controllers.SoccerFieldInfo
{
    [Route("api/v1/price-menus")]
    [ApiController]
    [Authorize]
    public class PriceMenusController : ControllerBase
    {
        private readonly BookingSoccersContext bookingSoccersContext;
        private readonly IPriceMenuService priceMenuService;
        private readonly IMapper mapper;

        public PriceMenusController(BookingSoccersContext bookingSoccersContext,
            IPriceMenuService priceMenuService, IMapper mapper)
        {
            this.priceMenuService = priceMenuService;
            this.bookingSoccersContext = bookingSoccersContext;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        //Get price menus as list
        public async Task<IActionResult> GetPriceMenusList
            ([FromQuery] PagingPayload pagingPayload,
            [FromQuery] PriceMenuPredicate predicate)
        {

            var result = await priceMenuService.RetrievePriceMenusList
                (pagingPayload, predicate);

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }

        [Authorize(Roles = "Admin, FieldManager")]
        [HttpGet("{id}")]
        //Get details of a price menu
        public async Task<IActionResult> GetAPriceMenuDetails(int id)
        {
            var retrievedPriceMenu = await priceMenuService.GetAPriceMenuDetails(id);

            if (retrievedPriceMenu.IsSuccess)
                return Ok(retrievedPriceMenu);

            Response.StatusCode = retrievedPriceMenu.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedPriceMenu);

            return StatusCode(retrievedPriceMenu.StatusCode, response);
        }

        [Authorize(Roles ="FieldManager,Admin")]
        [HttpPost]
        //Create a new price menu
        public async Task<IActionResult> AddNewPriceMenu(PriceMenuCreatePayload newPriceMenuInfo)
        {
            var AddedPriceMenu = await priceMenuService.AddANewPriceMenu(newPriceMenuInfo);

            if (AddedPriceMenu.IsSuccess)
                return Ok(AddedPriceMenu);

            Response.StatusCode = AddedPriceMenu.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedPriceMenu);

            return StatusCode(AddedPriceMenu.StatusCode, response);
        }

        // [Authorize(Roles ="FieldManager,Admin")]
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateAPriceMenu(int id,
        //    PriceMenuUpdatePayload NewPriceMenuInfo)
        //{

        //    var updatedPriceMenu = await priceMenuService.UpdateAPriceMenu(id, NewPriceMenuInfo);

        //    if (updatedPriceMenu.IsSuccess)
        //        return Ok(updatedPriceMenu);

        //    Response.StatusCode = updatedPriceMenu.StatusCode;

        //    var response = mapper.Map<ErrorResponse>(updatedPriceMenu);

        //    return StatusCode(updatedPriceMenu.StatusCode, response);
        //}

        [Authorize(Roles = "FieldManager,Admin")]
        [HttpPut("{id}")]
        //Update an existing price menu
        public async Task<IActionResult> UpdatePriceMenu(int id,
            PriceMenuUpdatePayload NewPriceMenuInfo)
        {

            var updatedPriceMenu = await priceMenuService.UpdatePriceMenu1
                (id, NewPriceMenuInfo);

            if (updatedPriceMenu.IsSuccess)
                return Ok(updatedPriceMenu);

            Response.StatusCode = updatedPriceMenu.StatusCode;

            var response = mapper.Map<ErrorResponse>(updatedPriceMenu);

            return StatusCode(updatedPriceMenu.StatusCode, response);
        }

         [Authorize(Roles ="FieldManager,Admin")]
        [HttpDelete("{id}")]
        //Remove an existing price menu 
        public async Task<IActionResult> DeleteAPriceMenu(int id)
        {
            var deletedPriceMenu = await priceMenuService.RemoveAPriceMenu(id);

            if (deletedPriceMenu.IsSuccess)
                return Ok(deletedPriceMenu);

            Response.StatusCode = deletedPriceMenu.StatusCode;

            var response = mapper.Map<ErrorResponse>(deletedPriceMenu);

            return StatusCode(deletedPriceMenu.StatusCode, response);
        }
    }
}
