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
        public int FieldId { get; set; }

        public byte ZoneTypeId { get; set; }

        [Required(ErrorMessage = "DateType cannot be null or empty")]
        public DayTypeEnum DayType { get; set; }

        [Required(ErrorMessage = "StartDate cannot be null or empty")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate cannot be null or empty")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status cannot be null or empty")]
        public byte Status { get; set; }

    }
}
