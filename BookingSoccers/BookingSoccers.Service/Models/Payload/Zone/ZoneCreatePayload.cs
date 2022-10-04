using System.ComponentModel.DataAnnotations;


namespace BookingSoccers.Service.Models.Payload.Zone
{
    public class ZoneCreatePayload
    {
        [Required]
        public int FieldId { get; set; }

        [Required]
        public byte ZoneTypeId { get; set; }

        [Required]
        public byte Number { get; set; }

        public int Width { get; set; }

        public int Length { get; set; }

        public int Area { get; set; }
    }
}
