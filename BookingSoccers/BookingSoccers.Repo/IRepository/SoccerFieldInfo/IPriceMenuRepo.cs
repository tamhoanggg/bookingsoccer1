using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.IRepository.SoccerFieldInfo
{
    public interface IPriceMenuRepo : IBaseRepository<PriceMenu>
    {
        Task<List<PriceMenu>> GetPriceMenusForAField(int FieldId);

        Task<PriceMenu> GetAPriceMenu(int FieldId, DateTime date, byte ZoneTypeId);

        Task<PriceMenu> GetAPriceMenuDetails(int PriceMenuId);

        Task<PriceMenu> GetFieldOpeningHour(int MenuId);
    }
}
