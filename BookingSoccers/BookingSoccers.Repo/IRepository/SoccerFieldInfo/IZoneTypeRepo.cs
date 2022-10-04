using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.IRepository.SoccerFieldInfo
{
    public interface IZoneTypeRepo : IBaseRepository<ZoneType>
    {
        Task<ZoneType> GetZoneTypeByName(string TypeName); 
    }
}
