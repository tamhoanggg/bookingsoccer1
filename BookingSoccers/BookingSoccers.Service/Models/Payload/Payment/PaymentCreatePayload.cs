using BookingSoccers.Repo.Entities.BookingInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.Payment
{
    public class PaymentCreatePayload
    {
        [RegularExpression("^[0-9]+$", ErrorMessage = "BookingId is an Positive Integer.")]
        public int BookingId { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "ReceiverId is an Positive Integer.")]
        public int ReceiverId { get; set; }

        [Range(1, 2, ErrorMessage = "PaymentTypeEnum must be between 0 and 1.")]
        public PaymentTypeEnum Type { get; set; }

        [Range(1000, 100000, ErrorMessage = "Amount  must be between 0 and 23.")]
        public int Amount { get; set; }
        
        public DateTime Time { get; set; }
    }
}
