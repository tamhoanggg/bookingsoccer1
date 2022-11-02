using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.ZoneType
{
    public class ZoneTypeCreatePayload
    {
        [Required(ErrorMessage = "Name cannot be null or empty")]
        [StringLength(15,ErrorMessage ="Name must contain at least 1 character and less than 15 character",MinimumLength =1)]
        public string Name { get; set; }

        [Required(ErrorMessage = "DepositPercent cannot be null or empty")]
        [Range(10, 50, ErrorMessage = "Value  must be between 10 and 50.")]
        public byte DepositPercent { get; set; }
    }
}
