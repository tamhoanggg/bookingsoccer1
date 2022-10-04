using System.ComponentModel.DataAnnotations;

namespace BookingSoccers.Service.Models.Payload.ZoneSlot
{
    public class ZoneSlotCreatePayload
    {
        [Required]
        public int ZoneId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public byte Status { get; set; }
    }
}
