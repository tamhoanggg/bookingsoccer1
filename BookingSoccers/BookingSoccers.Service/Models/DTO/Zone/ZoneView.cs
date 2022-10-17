using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSoccers.Service.Models.DTO.ZoneSlot;

namespace BookingSoccers.Service.Models.DTO.Zone
{
    public class ZoneView
    {
        public byte ZoneNumber { get; set; }

        public byte ZoneType { get; set; }

        public ICollection<ZoneSlotView> ZoneTypeSlots { get; set; }
    }
}
