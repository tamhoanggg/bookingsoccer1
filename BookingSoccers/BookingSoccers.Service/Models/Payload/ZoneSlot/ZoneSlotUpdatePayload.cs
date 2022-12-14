using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.ZoneSlot
{
    public class ZoneSlotUpdatePayload
    {
        [Required(ErrorMessage = "ZoneId cannot be null or empty")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "ZoneID is an Positive Integer.")]
        public int ZoneId { get; set; }

        [Required(ErrorMessage = "StartTime cannot be null or empty")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "EndTime cannot be null or empty")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Status cannot be null or empty")]
        [Range(0, 1, ErrorMessage = "status between 0 to 1.")]
        public byte Status { get; set; }
    }
}
