using BookingSoccers.Repo.Entities.BookingInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.Booking
{
    public class BookingCreateForm
    {
        public string UserName { get; set; }

        public string ZoneType { get; set; }

        public string FieldName { get; set; }

        public int ZoneId { get; set; }

        public string Address { get; set; }

        public int TotalPrice { get; set; }

        public int DepositPercent { get; set; }

        public int PrepayAmount { get; set; }

        public DateTime HireDate { get; set; }

        public int StartTimeHour { get; set; }

        public int StartTimeMinute { get; set; }

        public int EndTimeHour { get; set; }

        public int EndTimeMinute { get; set; }

        public StatusEnum Status { get; set; }

        public List<int> SlotsIdList { get; set; }
    }
}
