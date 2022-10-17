using BookingSoccers.Service.Models.DTO.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.DTO.SoccerField
{
    public class SoccerFieldSalesReport
    {
        public ICollection<Booking14DaysReportView> Booking14DaysReport { get; set; }

        public ICollection<Booking6MonthReportView> Booking6MonthReports { get; set; }

        public CustomBookingReportView CustomBookingReport { get; set; }
    }
}
