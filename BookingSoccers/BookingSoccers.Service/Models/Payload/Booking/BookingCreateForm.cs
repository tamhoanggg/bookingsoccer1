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
        [RegularExpression("^[a-zA-Z]+$",ErrorMessage ="UserName was character Only")]
        public string UserName { get; set; }

        public string ZoneType { get; set; }

        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = " was character Only")]
        public string FieldName { get; set; }

        public int ZoneId { get; set; }

        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Address was character Only")]
        public string Address { get; set; }

        [Range(50000,10000000,ErrorMessage ="Total Price must between 50000 and 1000000")]
        public int TotalPrice { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "DepositPercent is an Positive Integer.")]
        public int DepositPercent { get; set; }

        [Range(1000, 1000000, ErrorMessage = "Total Price must between 1000 and 100000")]
        public int PrepayAmount { get; set; }

        public DateTime HireDate { get; set; }

        [Range(0, 23, ErrorMessage = "StartTimeHour  must be between 0 and 23.")]
        public int StartTimeHour { get; set; }

        [Range(0, 59, ErrorMessage = "StartTimeMinute must be between 0 and 59.")]
        public int StartTimeMinute { get; set; }

        [Range(0, 23, ErrorMessage = "EndTimeHour  must be between 0 and 23.")]
        public int EndTimeHour { get; set; }

        [Range(0, 59, ErrorMessage = "EndTimeMinute must be between 0 and 59.")]
        public int EndTimeMinute { get; set; }

        [Range(0, 1, ErrorMessage = "Status has only 0 and 1.")]
        public StatusEnum Status { get; set; }

        public List<int> SlotsIdList { get; set; }
    }
}
