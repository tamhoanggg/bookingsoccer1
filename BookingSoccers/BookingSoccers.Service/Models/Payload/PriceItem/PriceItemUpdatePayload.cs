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
        [RegularExpression("^[0-9]+$", ErrorMessage = "PriceMenuId is an Positive Integer.")]
        public int PriceMenuId { get; set; }

        [Required(ErrorMessage = "StartTimeHour cannot be null or empty")]
        [Range(0, 23, ErrorMessage = "StartTimeHour  must be between 0 and 23.")]
        public int StartTimeHour { get; set; }

        [Required(ErrorMessage = "StartTimeMinute cannot be null or empty")]
        [Range(0, 59, ErrorMessage = "StartTimeMinute  must be between 0 and 59.")]
        public int StartTimeMinute { get; set; }

        [Required(ErrorMessage = "EndTimeHour cannot be null or empty")]
        [Range(0, 23, ErrorMessage = "EndTimeHour  must be between 0 and 23.")]
        public int EndTimeHour { get; set; }

        [Required(ErrorMessage = "EndTimeMinute cannot be null or empty")]
        [Range(0, 59, ErrorMessage = "EndTimeMinute  must be between 0 and 59.")]
        public int EndTimeMinute { get; set; }

        [Required(ErrorMessage = "Price cannot be null or empty")]
        [Range(10000, 500000,
        ErrorMessage = "Hiring price must be greater than 10k VND and less than 500k VND")]
        public int Price { get; set; }

    }
}
