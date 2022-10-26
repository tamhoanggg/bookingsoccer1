using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.ZoneSlot
{
    public class ZoneSlotPredicate
    {
        public DateTime? StartTimeFrom { get; set; }

        public DateTime? StartTimeTo { get; set; }

        public DateTime? EndTimeFrom { get; set; }

        public DateTime? EndTimeTo { get; set; }

        public byte? Status { get; set; }
    }
}
