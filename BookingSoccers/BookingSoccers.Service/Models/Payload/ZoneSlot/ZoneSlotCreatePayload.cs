using System.ComponentModel.DataAnnotations;

namespace BookingSoccers.Service.Models.Payload.ZoneSlot
{
    public class ZoneSlotCreatePayload
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
