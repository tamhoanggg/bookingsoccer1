using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.PriceItem
{
    public class PriceItemPredicate
    {
        public int? StartTimeHour { get; set; }

        public int? StartTimeMinute { get; set; }

        public int? EndTimeHour { get; set; }

        public int? EndTimeMinute { get; set; }

        public int? FromPrice { get; set; }

        public int? ToPrice { get; set; }
    }
}
