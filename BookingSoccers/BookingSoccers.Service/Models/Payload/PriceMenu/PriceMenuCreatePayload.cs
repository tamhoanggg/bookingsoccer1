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

        [Required]
        public DayTypeEnum DayType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public byte Status { get; set; }

    }
}
