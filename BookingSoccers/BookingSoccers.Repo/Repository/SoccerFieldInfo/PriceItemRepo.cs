using BookingSoccers.Repo.Context;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Repo.IRepository.SoccerFieldInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.Repository.SoccerFieldInfo
{
    public class PriceItemRepo : BaseRepository<PriceItem>, IPriceItemRepo
    {
        BookingSoccersContext bookingSoccersContext;
        public PriceItemRepo(BookingSoccersContext _bookingSoccersContext) : base(_bookingSoccersContext)
        {
            bookingSoccersContext = _bookingSoccersContext;
        }

        public async Task<PriceItem> getAPriceItemDetail(int id)
        {
            var returnedPriceItem = await Get()
                .Include(x => x.Menu)
                .ThenInclude(x => x.Field)
                .Include(x => x.Menu)
                .ThenInclude(x => x.TypeOfZone)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return returnedPriceItem;
        }

        public async Task<PriceItem> getFieldViaPriceItem(int Id)
        {
            var returnPriceItem = await Get()
                .Include(x => x.Menu)
                .ThenInclude(y => y.Field)
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();

            return returnPriceItem;
        }
    }
}
