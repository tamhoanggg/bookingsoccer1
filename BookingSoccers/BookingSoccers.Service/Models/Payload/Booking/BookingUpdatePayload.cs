using BookingSoccers.Repo.Entities.BookingInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.Booking
{
    public class BookingUpdatePayload
    {
        [Required(ErrorMessage = "CustomerID cannot be null or empty")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "CustomerID is an Positive Integer.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "ZoneTypeID cannot be null or empty")]
        [Range(1, 3, ErrorMessage = "ZoneTypeID between 1 to 3")]
        public byte ZoneTypeId { get; set; }

        [Required(ErrorMessage = "ZoneID cannot be null or empty")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "ZoneID is an Positive Integer.")]
        public int ZoneId { get; set; }

        [Required(ErrorMessage = "FieldID cannot be null or empty")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "FieldID is an Positive Integer.")]
        public int FieldId { get; set; }


        [Required(ErrorMessage = "Total price cannot be null or empty")]
        [Range(50000, 1000000, ErrorMessage = "Total Price must between 50000 and 1000000")]
        public int TotalPrice { get; set; }

        [Required(ErrorMessage = "StartTime cannot be null or empty")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "EndTime cannot be null or empty")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "CreateTime cannot be null or empty")]
        public DateTime CreateTime { get; set; }

        [Required(ErrorMessage = "Status cannot be null or empty")]
        [Range(1, 4, ErrorMessage = "Status value must be between 1 to 5")]
        public StatusEnum Status { get; set; }

        [Required(ErrorMessage = "Rating cannot be null or empty")]
        [Range(1, 5, ErrorMessage = "Rating value between 1 to 5")]
        public byte Rating { get; set; }


        [Required(ErrorMessage = "Comment cannot be null or empty")]
        [StringLength(1000, MinimumLength = 0, ErrorMessage = "Comment must be lesser than 100 characters")]
        public string? Comment { get; set; }
    }
}
