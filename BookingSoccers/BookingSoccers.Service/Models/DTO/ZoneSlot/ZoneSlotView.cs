using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.DTO.ZoneSlot
{
    public class ZoneSlotView
    {
        public TimeSpan SlotStartTime { get; set; }

        public TimeSpan SlotEndTime { get; set; }

        public byte Status { get; set; }
    }
}
