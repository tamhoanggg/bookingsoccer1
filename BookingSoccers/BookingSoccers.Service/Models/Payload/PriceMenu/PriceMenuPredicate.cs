using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.PriceMenu
{
    public class PriceMenuPredicate
    {
        public byte? ZoneTypeId { get; set; }
        
        public int? DayType { get; set; }

        public DateTime? StartDateFrom { get; set; }

        public DateTime? StartDateTo { get; set; }

        public DateTime? EndDateFrom { get; set; }

        public DateTime? EndDateTo { get; set; }

        public byte? Status { get; set; }
    }
}
