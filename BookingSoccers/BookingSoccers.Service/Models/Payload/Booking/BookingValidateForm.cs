using BookingSoccers.Repo.Entities.BookingInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.Booking
{
    public class BookingValidateForm
    {
        [Required(ErrorMessage = "ZoneTypeID cannot be null or empty")]
        [Range(1, 3, ErrorMessage = "ZoneTypeID between 1 to 3")]
        public byte ZoneTypeId { get; set; }

        [Required(ErrorMessage = "FieldID cannot be null or empty")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "FieldID is an Positive Integer.")]
        public int FieldId { get; set; }

        [Required(ErrorMessage = "Address cannot be null or empty")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Address was character Only")]
        public string Address { get; set; }

        [Required(ErrorMessage = "BookingDate cannot be null or empty")]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "StartTimeHour cannot be null or empty")]
        public int StartTimeHour { get; set; }

        [Required(ErrorMessage = "StartTimeMinute cannot be null or empty")]
        public int StartTimeMinute { get; set; }

        [Required(ErrorMessage = "HireAMount cannot be null or empty")]
        [RegularExpression("^60|120$",ErrorMessage ="HireAmount value was only 60 or 120")]
        public int HireAmount { get; set; }

    }
}
