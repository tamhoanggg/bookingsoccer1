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
        public int ManagerId { get; set; }

        [Required(ErrorMessage = "FieldName cannot be null or empty")]
        [StringLength(0, ErrorMessage = "Name must contain at least 1 character and less than 100 character", MinimumLength = 100)]
        public string FieldName { get; set; }

        [Required(ErrorMessage = "Description cannot be null or empty")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ImageFolderPath cannot be null or empty")]
        public string ImageFolderPath { get; set; }

        [Required(ErrorMessage = "OpenHour cannot be null or empty")]
        public int OpenHour { get; set; }

        [Required(ErrorMessage = "OpenMinute cannot be null or empty")]
        public int OpenMinute { get; set; }

        [Required(ErrorMessage = "CloseHour cannot be null or empty")]
        public int CloseHour { get; set; }

        [Required(ErrorMessage = "CloseMinute cannot be null or empty")]
        public int CloseMinute { get; set; }

        [Required(ErrorMessage = "Address cannot be null or empty")]
        [StringLength(0, ErrorMessage = "Name must contain at least 1 character and less than 100 character", MinimumLength = 100)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Status cannot be null or empty")]
        public byte Status { get; set; }

        public string IdToken { get; set; }

    }
}
