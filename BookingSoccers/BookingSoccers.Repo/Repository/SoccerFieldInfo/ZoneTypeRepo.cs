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
    public class ZoneTypeRepo : BaseRepository<ZoneType>, IZoneTypeRepo
    {
        BookingSoccersContext bookingSoccersContext;
        public ZoneTypeRepo(BookingSoccersContext _bookingSoccersContext) : base(_bookingSoccersContext)
        {
            bookingSoccersContext = _bookingSoccersContext;
        }

        public Task<ZoneType> GetZoneTypeByName(string TypeName)
        {
            var foundZoneType = Get().Where(x => x.Name == TypeName).FirstOrDefaultAsync();
            return foundZoneType;
        }
    }
}
