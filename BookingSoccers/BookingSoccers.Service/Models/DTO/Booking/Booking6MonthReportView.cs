using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.DTO.Booking
{
    public class Booking6MonthReportView
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public int SalesSum { get; set; }

        public int BookingCount { get; set; }
    }
}
