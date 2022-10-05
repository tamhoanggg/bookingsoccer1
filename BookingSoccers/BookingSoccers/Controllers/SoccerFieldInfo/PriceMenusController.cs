using AutoMapper;
using BookingSoccers.Repo.Context;
using BookingSoccers.Service.IService.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.Payload.PriceMenu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingSoccers.Controllers.SoccerFieldInfo
{
    [Route("api/price-menus")]
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

        [HttpGet]
        public async Task<IActionResult> GetPriceMenus()
        {

            var result = await priceMenuService.RetrieveAllPriceMenus();

            if (result.IsSuccess)
                return Ok(result);

            Response.StatusCode = result.StatusCode;

            var response = mapper.Map<ErrorResponse>(result);
            return StatusCode(result.StatusCode, response);
        }
         [Authorize(Roles ="FieldManager,Admin")]
        [HttpPost]
        public async Task<IActionResult> AddNewPriceMenu(PriceMenuCreatePayload newPriceMenuInfo)
        {
            var AddedPriceMenu = await priceMenuService.AddANewPriceMenu(newPriceMenuInfo);

            if (AddedPriceMenu.IsSuccess)
                return Ok(AddedPriceMenu);

            Response.StatusCode = AddedPriceMenu.StatusCode;

            var response = mapper.Map<ErrorResponse>(AddedPriceMenu);

            return StatusCode(AddedPriceMenu.StatusCode, response);
        }
         [Authorize(Roles ="FieldManager,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAPriceMenu(int id,
            PriceMenuUpdatePayload NewPriceMenuInfo)
        {

            var updatedPriceMenu = await priceMenuService.UpdateAPriceMenu(id, NewPriceMenuInfo);

            if (updatedPriceMenu.IsSuccess)
                return Ok(updatedPriceMenu);

            Response.StatusCode = updatedPriceMenu.StatusCode;

            var response = mapper.Map<ErrorResponse>(updatedPriceMenu);

            return StatusCode(updatedPriceMenu.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneSpecificPriceMenu(int id)
        {
            var retrievedPriceMenu = await priceMenuService.RetrieveAPriceMenuById(id);

            if (retrievedPriceMenu.IsSuccess)
                return Ok(retrievedPriceMenu);

            Response.StatusCode = retrievedPriceMenu.StatusCode;

            var response = mapper.Map<ErrorResponse>(retrievedPriceMenu);

            return StatusCode(retrievedPriceMenu.StatusCode, response);
        }
         [Authorize(Roles ="FieldManager,Admin")]
        [HttpDelete("{id}")]
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
