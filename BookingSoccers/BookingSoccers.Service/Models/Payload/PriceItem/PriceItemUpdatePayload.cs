using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.PriceItem
{
    public class PriceItemUpdatePayload
    {
        public int PriceMenuId { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        [Range(10000, 500000,
        ErrorMessage = "Hiring price must be greater than 10k VND and less than 500k VND")]
        public int Price { get; set; }

        [Required]
        public byte TimeAmount { get; set; }
    }
}
