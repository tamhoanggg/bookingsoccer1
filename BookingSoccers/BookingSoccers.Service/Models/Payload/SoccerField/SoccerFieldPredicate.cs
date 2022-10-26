using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.SoccerField
{
    public class SoccerFieldPredicate
    {
        public int? OpenTimeHour { get; set; }

        public int? OpenTimeMinute { get; set; }

        public int? CloseTimeHour { get; set; }

        public int? CloseTimeMinute { get; set; }

        public byte? Status { get; set; }

    }
}
