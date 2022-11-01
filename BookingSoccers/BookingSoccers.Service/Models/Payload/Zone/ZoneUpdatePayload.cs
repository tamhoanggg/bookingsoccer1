using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.Zone
{
    public class ZoneUpdatePayload
    {
        [Required(ErrorMessage = "FieldID cannot be null or empty")]
        public int FieldId { get; set; }

        [Required(ErrorMessage = "ZoneTypeId cannot be null or empty")]
        public byte ZoneTypeId { get; set; }

        [Required(ErrorMessage = "Number cannot be null or empty")]
        public byte Number { get; set; }

        public int Width { get; set; }

        public int Length { get; set; }

        public int Area { get; set; }
    }
}
