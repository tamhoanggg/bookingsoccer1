using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.DTO.Booking
{
    public class Booking14DaysReportView
    {
        public int Day { get; set; }

        public int Month { get; set; }

        public int DaySalesSum { get; set; }

        public int BookingCount { get; set; }
    }
}
