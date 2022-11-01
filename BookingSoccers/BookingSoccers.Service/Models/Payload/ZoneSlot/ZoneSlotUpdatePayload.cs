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
        public int ZoneId { get; set; }

        [Required(ErrorMessage = "StartTime cannot be null or empty")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "EndTime cannot be null or empty")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Status cannot be null or empty")]
        public byte Status { get; set; }
    }
}
