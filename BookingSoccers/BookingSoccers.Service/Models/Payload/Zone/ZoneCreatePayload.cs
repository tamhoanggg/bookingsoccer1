using System.ComponentModel.DataAnnotations;


namespace BookingSoccers.Service.Models.Payload.Zone
{
    public class ZoneCreatePayload
    {
        [Required(ErrorMessage = "FieldID cannot be null or empty")]
        public int FieldId { get; set; }

        [Required(ErrorMessage = "ZoneTypeID cannot be null or empty")]
        public byte ZoneTypeId { get; set; }

        [Required(ErrorMessage = "Number cannot be null or empty")]
        public byte Number { get; set; }

        public int Width { get; set; }

        public int Length { get; set; }

        public int Area { get; set; }
    }
}
