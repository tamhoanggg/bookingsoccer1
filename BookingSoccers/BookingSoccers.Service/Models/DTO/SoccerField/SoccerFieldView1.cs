using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using BookingSoccers.Service.Models.DTO.PriceMenu;
using BookingSoccers.Service.Models.DTO.Zone;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.DTO.SoccerField
{
    public class SoccerFieldView1
    {
        public int Id { get; set; }

        public ICollection<PriceMenuView> PriceMenusList { get; set; }

        public ICollection<ZoneView> ZonesList { get; set; }

        public string FieldName { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public TimeSpan OpenHour { get; set; }

        public TimeSpan CloseHour { get; set; }

        public string Address { get; set; }

        public byte Status { get; set; }

        public int TotalReviews { get; set; }

        public int AverageReviewScore { get; set; }
    }
}
