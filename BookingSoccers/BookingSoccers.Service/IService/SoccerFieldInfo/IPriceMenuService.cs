using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.ImageFolder;
using BookingSoccers.Service.Models.Payload.PriceMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.SoccerFieldInfo
{
    public interface IPriceMenuService
    {
        Task<GeneralResult<PriceMenu>> AddANewPriceMenu(
            PriceMenuCreatePayload priceMenuInfo);

        Task<GeneralResult<Object>> GetAPriceMenuDetails(int priceMenuId);

        Task<GeneralResult<ObjectListPagingInfo>> RetrievePriceMenusList
            (PagingPayload pagingPayload, PriceMenuPredicate filter);

        Task<GeneralResult<PriceMenu>> UpdateAPriceMenu(int Id, PriceMenuUpdatePayload newPriceMenuInfo);

        Task<GeneralResult<PriceMenu>> UpdatePriceMenu1
            (int Id, PriceMenuUpdatePayload updateInfo);

        Task<GeneralResult<PriceMenu>> RemoveAPriceMenu(int PriceMenuId);
    }
}
