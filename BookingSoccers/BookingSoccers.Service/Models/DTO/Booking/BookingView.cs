using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Service.Models.DTO.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.DTO.Booking
{
    public class BookingView
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string ZoneTypeName { get; set; }

        public int ZoneNumber { get; set; }
        
        public ICollection<PaymentView> paymentsList { get; set; }

        public string FieldName { get; set; }

        public string FieldAddress { get; set; }

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
