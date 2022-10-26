using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.Booking
{
    public class BookingPredicate
    {
        public int? UserId { get; set; }

        public byte? ZoneTypeId { get; set; }

        public int? FromPrice { get; set; }

        public int? ToPrice { get; set; }

        public DateTime? BookingDateFrom { get; set; }

        public DateTime? BookingDateTo { get; set; }

        public DateTime? CreateTimeFrom { get; set; }

        public DateTime? CreateTimeTo { get; set; }

        public int? Status { get; set; }

        public byte? Rating { get; set; }
    }
}
