using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.SoccerField
{
    public class SoccerFieldZoneSlots
    {
        [Required(ErrorMessage = "FieldID cannot be null or empty")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "FieldID is an Positive Integer.")]
        public int FieldId { get; set; }

        [Required(ErrorMessage = "Date cannot be null or empty")]
        public DateTime Date { get; set; }
    }
}
