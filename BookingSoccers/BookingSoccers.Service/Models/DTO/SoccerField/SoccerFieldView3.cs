using BookingSoccers.Service.Models.DTO.PriceMenu;
using BookingSoccers.Service.Models.DTO.Zone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.DTO.SoccerField
{
    public class SoccerFieldView3
    {
        public int Id { get; set; }

        public string ContactNumber { get; set; }

        public string FieldName { get; set; }

        public string Description { get; set; }

        public string ImageFolderPath { get; set; }

        public TimeSpan OpenHour { get; set; }

        public TimeSpan CloseHour { get; set; }

        public string Address { get; set; }

        public byte Status { get; set; }

    }
}
