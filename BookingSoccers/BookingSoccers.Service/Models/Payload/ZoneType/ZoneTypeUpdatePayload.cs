using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.ZoneType
{
    public class ZoneTypeUpdatePayload
    {
        [Required(ErrorMessage = "Name cannot be null or empty")]
        [StringLength(0, ErrorMessage = "Name must contain at least 1 character and less than 15 character", MinimumLength = 15)]
        public string Name { get; set; }

        [Required(ErrorMessage = "DepositPercent cannot be null or empty")]
        [Range(10, 50, ErrorMessage = "Value  must be between 10 and 50.")]
        public byte DepositPercent { get; set; }
    }
}
