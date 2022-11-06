using BookingSoccers.Repo.Entities.BookingInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.Booking
{
    public class BookingCreateForm
    {
        [Required(ErrorMessage = "UserName cannot be null or empty")]
        [RegularExpression("^[a-zA-Z0-9]+$", 
            ErrorMessage ="UserName must consist of uppper case " +
            "or lowercase characters and numbers and not special characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "ZoneType cannot be null or empty")]
        public string ZoneType { get; set; }

        [Required(ErrorMessage = "FieldName cannot be null or empty")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "FieldName must be between 1 and 100 characters")]
        public string FieldName { get; set; }

        [Required(ErrorMessage = "ZoneID cannot be null or empty")]
        public int ZoneId { get; set; }

        [Required(ErrorMessage = "Address cannot be null or empty")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Address must be between 1 and 100 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Total price cannot be null or empty")]
        [Range(50000,1000000,ErrorMessage ="Total Price must between 50000 and 1000000")]
        public int TotalPrice { get; set; }

        [Required(ErrorMessage = "DepositPercent cannot be null or empty")]
        [Range(10, 50, ErrorMessage = "DepositPercent must be between 10 and 50")]
        public int DepositPercent { get; set; }

        [Required(ErrorMessage = "PrepayAmount cannot be null or empty")]
        [Range(1000, 100000, ErrorMessage = "Total Price must between 1000 and 100000")]
        public int PrepayAmount { get; set; }

        [Required(ErrorMessage = "HireDate cannot be null or empty")]
        public DateTime HireDate { get; set; }

        [Required(ErrorMessage = "StartTimeHour cannot be null or empty")]
        [Range(0, 23, ErrorMessage = "StartTimeHour  must be between 0 and 23.")]
        public int StartTimeHour { get; set; }

        [Required(ErrorMessage = "StartTimeMinute cannot be null or empty")]
        [Range(0, 59, ErrorMessage = "StartTimeMinute must be between 0 and 59.")]
        public int StartTimeMinute { get; set; }

        [Required(ErrorMessage = "EndTimeHour cannot be null or empty")]
        [Range(0, 23, ErrorMessage = "EndTimeHour  must be between 0 and 23.")]
        public int EndTimeHour { get; set; }

        [Required(ErrorMessage = "EndTimeMinute cannot be null or empty")]
        [Range(0, 59, ErrorMessage = "EndTimeMinute must be between 0 and 59.")]
        public int EndTimeMinute { get; set; }

        [Required(ErrorMessage = "Status cannot be null or empty")]
        [Range(0, 1, ErrorMessage = "Status has only 0 and 1.")]
        public StatusEnum Status { get; set; }

        public List<int> SlotsIdList { get; set; }
    }
}
