using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSoccers.Service.Models.DTO.PriceItem;

namespace BookingSoccers.Service.Models.DTO.PriceMenu
{
    public class PriceMenuView
    {
        public int Id { get; set; }

        public ICollection<PriceItemView> PriceItemsList { get; set; }

        public string ZoneType { get; set; }

        public DayTypeEnum DayType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public byte Status { get; set; }
    }
}
