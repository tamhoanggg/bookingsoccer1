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
    public class ZoneRepo : BaseRepository<Zone>, IZoneRepo
    {
        BookingSoccersContext bookingSoccersContext;
        public ZoneRepo(BookingSoccersContext _bookingSoccersContext) : base(_bookingSoccersContext)
        {
            bookingSoccersContext = _bookingSoccersContext;
        }

        public async Task<List<Zone>> getFieldZonesByFieldId(int FieldId)
        {
            var IdList = await Get()
                        .Where(x => x.FieldId == FieldId)
                        .ToListAsync();

            return IdList;        
        }

        public async Task<List<Zone>> getZonesByZoneType(int FieldId, byte ZoneType)
        {
            var ZoneList = await Get()
                .Where(x => x.ZoneTypeId == ZoneType && x.FieldId == FieldId)
                .ToListAsync();

            return ZoneList;
        }
    }
}
