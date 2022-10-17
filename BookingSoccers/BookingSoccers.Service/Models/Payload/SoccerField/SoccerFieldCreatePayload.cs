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

        [Required]
        [StringLength(100)]
        public string FieldName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageFolderPath { get; set; }

        [Required]
        public int OpenHour { get; set; }

        [Required]
        public int OpenMinute { get; set; }

        [Required]
        public int CloseHour { get; set; }

        [Required]
        public int CloseMinute { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        public byte Status { get; set; }

        public string IdToken { get; set; }

    }
}
