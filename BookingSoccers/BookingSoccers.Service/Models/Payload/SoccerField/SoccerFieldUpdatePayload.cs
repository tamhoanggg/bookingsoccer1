using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.SoccerField
{
    public class SoccerFieldUpdatePayload
    {
        public int ManagerId { get; set; }

        [Required(ErrorMessage = "FieldName cannot be null or empty")]
        [StringLength(100, ErrorMessage = "Field Name must contain at least 1 character and less than 100 character", MinimumLength = 1)]
        public string FieldName { get; set; }

        [Required(ErrorMessage = "Description cannot be null or empty")]
        public string Description { get; set; }

        [Required(ErrorMessage = "OpenTimeHour cannot be null or empty")]
        public int OpenTimeHour { get; set; }

        [Required(ErrorMessage = "OpenTimeMinute cannot be null or empty")]
        public int OpenTimeMinute { get; set; }

        [Required(ErrorMessage = "CloseTimeHour cannot be null or empty")]
        public int CloseTimeHour { get; set; }

        [Required(ErrorMessage = "CloseTimeMinute cannot be null or empty")]
        public int CloseTimeMinute { get; set; }


        [Required(ErrorMessage = "Address cannot be null or empty")]
        [StringLength(100, ErrorMessage = "Address must contain at least 1 character and less than 100 character", MinimumLength = 1)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Status cannot be null or empty")]
        public byte Status { get; set; }

        [Required(ErrorMessage = "TotalReviews cannot be null or empty")]
        public int TotalReviews { get; set; }

        [Required(ErrorMessage = "ReviewScoreSum cannot be null or empty")]
        public int ReviewScoreSum { get; set; }

    }
}
