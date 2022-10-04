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
        public int BookingId { get; set; }

        public int ReceiverId { get; set; }

        public PaymentTypeEnum Type { get; set; }

        public int Amount { get; set; }

        public DateTime Time { get; set; }
    }
}
