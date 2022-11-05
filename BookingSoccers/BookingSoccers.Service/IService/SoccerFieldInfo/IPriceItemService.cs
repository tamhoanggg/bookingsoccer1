using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO;
using BookingSoccers.Service.Models.Payload;
using BookingSoccers.Service.Models.Payload.ImageFolder;
using BookingSoccers.Service.Models.Payload.PriceItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService.SoccerFieldInfo
{
    public interface IPriceItemService
    {
        Task<GeneralResult<PriceItem>> AddANewPriceItem(
            PriceItemCreatePayload priceItemInfo);

        Task<GeneralResult<Object>> GetAPriceItemDetails(int priceItemId);

        Task<GeneralResult<ObjectListPagingInfo>> RetrievePriceItemsList
            (PagingPayload pagingPayload, PriceItemPredicate filter);

        Task<GeneralResult<PriceItem>> UpdateAPriceItem(int Id, PriceItemUpdatePayload newPriceItemInfo);

        Task<GeneralResult<PriceItem>> UpdateAPriceItem1
            (int Id, PriceItemUpdatePayload updateInfo);

        Task<GeneralResult<PriceItem>> RemoveAPriceItem(int PriceItemId);
    }
}
