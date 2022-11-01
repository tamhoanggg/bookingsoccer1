using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Repo.IRepository.SoccerFieldInfo
{
    public interface IZoneSlotRepo : IBaseRepository<ZoneSlot>
    {
        Task<List<ZoneSlot>> getZoneSlotsByZoneId(int Id, DateTime date);

        Task<List<ZoneSlot>> getZoneSlots(int Id, DateTime date);

        Task<ZoneSlot> getAZoneSlotByZoneId(int Id);

    }
}
