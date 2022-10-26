using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.Payment
{
    public class PaymentPredicate
    {
        public int? PaymentType { get; set; }

        public int? FromPrice { get; set; }

        public int? ToPrice { get; set; }

        public DateTime? CreateTimeFrom { get; set; }

        public DateTime? CreateTimeTo { get; set; }
    }
}
