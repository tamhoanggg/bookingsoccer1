using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.IRepository.SoccerFieldInfo
{
    public interface IZoneRepo : IBaseRepository<Zone>
    {
        Task<List<Zone>> getFieldZonesByFieldId(int FieldId);

        Task<List<Zone>> getZonesByZoneType(int FieldId, byte ZoneType);
    }
}
