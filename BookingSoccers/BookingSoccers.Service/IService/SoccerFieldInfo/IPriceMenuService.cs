using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.Common;
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

        Task<GeneralResult<PriceMenu>> RetrieveAPriceMenuById(int priceMenuId);

        Task<GeneralResult<List<PriceMenu>>> RetrieveAllPriceMenus();

        Task<GeneralResult<PriceMenu>> UpdateAPriceMenu(int Id, PriceMenuUpdatePayload newPriceMenuInfo);

        Task<GeneralResult<PriceMenu>> RemoveAPriceMenu(int PriceMenuId);
    }
}
