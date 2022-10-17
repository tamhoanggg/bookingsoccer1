using BookingSoccers.Service.Models.DTO.Booking;
using BookingSoccers.Service.Models.DTO.Zone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.DTO.SoccerField
{
    public class SoccerFieldView2
    {
        public int Id { get; set; }

        public ICollection<ZoneView1> Zones { get; set; }

        public ICollection<BookingView1> Bookings { get; set; }
    }
}
