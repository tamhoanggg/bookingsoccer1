using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.SoccerField
{
    public class SoccerFieldCreatePayload
    {
        [Required(ErrorMessage = "ManagerID cannot be null or empty")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "ManagerID is an Positive Integer.")]
        public int ManagerId { get; set; }

        [Required(ErrorMessage = "FieldName cannot be null or empty")]
        [StringLength(100, ErrorMessage = "Field Name must contain at least 1 character and less than 100 character", MinimumLength = 1)]
        public string FieldName { get; set; }

        [Required(ErrorMessage = "Description cannot be null or empty")]
        public string Description { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessage = "ImageFolderPath must be between 1 and 200 characters")]
        public string? ImageFolderPath { get; set; }

        [Required(ErrorMessage = "OpenHour cannot be null or empty")]
        [Range(0 , 23, ErrorMessage = "OpenHour  must be between 0 and 23.")]
        public int OpenHour { get; set; }

        [Required(ErrorMessage = "OpenMinute cannot be null or empty")]
        [Range(0, 59, ErrorMessage = "OpenMinute  must be between 0 and 59.")]
        public int OpenMinute { get; set; }

        [Required(ErrorMessage = "CloseHour cannot be null or empty")]
        [Range(0, 23, ErrorMessage = "OpenHour  must be between 0 and 23.")]
        public int CloseHour { get; set; }

        [Required(ErrorMessage = "CloseMinute cannot be null or empty")]
        [Range(0, 59, ErrorMessage = "CloseMinute  must be between 0 and 59.")]
        public int CloseMinute { get; set; }

        [Required(ErrorMessage = "Address cannot be null or empty")]
        [StringLength(100, ErrorMessage = "Address must contain at least 1 character and less than 100 character", MinimumLength = 1)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Status cannot be null or empty")]
        [Range(0, 1, ErrorMessage = "Status has value between 0 and 1")]
        public byte Status { get; set; }

        public string? IdToken { get; set; }

    }
}
