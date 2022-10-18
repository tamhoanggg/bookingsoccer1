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

        [Required]
        [StringLength(100)]
        public string FieldName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int OpenTimeHour { get; set; }

        [Required]
        public int OpenTimeMinute { get; set; }

        [Required]
        public int CloseTimeHour { get; set; }

        [Required]
        public int CloseTimeMinute { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        public byte Status { get; set; }

        [Required]
        public int TotalReviews { get; set; }

        [Required]
        public int ReviewScoreSum { get; set; }

    }
}
