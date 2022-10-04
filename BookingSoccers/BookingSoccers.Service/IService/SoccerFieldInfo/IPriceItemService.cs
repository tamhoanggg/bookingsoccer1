using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
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

        Task<GeneralResult<PriceItem>> RetrieveAPriceItemById(int priceItemId);

        Task<GeneralResult<List<PriceItem>>> RetrieveAllPriceItems();

        Task<GeneralResult<PriceItem>> UpdateAPriceItem(int Id, PriceItemUpdatePayload newPriceItemInfo);

        Task<GeneralResult<PriceItem>> RemoveAPriceItem(int PriceItemId);
    }
}
