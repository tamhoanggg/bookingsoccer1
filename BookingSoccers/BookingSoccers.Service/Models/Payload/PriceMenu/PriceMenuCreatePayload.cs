using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.PriceMenu
{
    public class PriceMenuCreatePayload
    {
        [RegularExpression("^[0-9]+$", ErrorMessage = "FieldID is an Positive Integer.")]
        public int FieldId { get; set; }

        [Range(1, 3, ErrorMessage = "ZoneTypeId  must be between 1 and 3.")]
        public byte ZoneTypeId { get; set; }

        [Required(ErrorMessage = "DateType cannot be null or empty")]
        public DayTypeEnum DayType { get; set; }

        [Required(ErrorMessage = "StartDate cannot be null or empty")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate cannot be null or empty")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status cannot be null or empty")]
        [Range(0, 1, ErrorMessage = "Status  must be between 0 and 1.")]
        public byte Status { get; set; }

    }
}
