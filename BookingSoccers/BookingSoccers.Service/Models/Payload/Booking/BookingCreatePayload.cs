using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.Booking
{
    public class BookingCreatePayload
    {
        public int CustomerId { get; set; }
        
        public byte ZoneTypeId { get; set; }
    
        public int ZoneId { get; set; }

        public int FieldId { get; set; }

        public int TotalPrice { get; set; }

        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }

        public DateTime CreateTime { get; set; }

        public StatusEnum Status { get; set; }

        public byte Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; }
    }
}
