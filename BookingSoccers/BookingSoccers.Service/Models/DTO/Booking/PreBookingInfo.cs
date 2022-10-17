using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.DTO.Booking
{
    public class PreBookingInfo
    {        
        public string ZoneType { get; set; }

        public string FieldName { get; set; }

        public int ZoneId { get; set; }

        public string Address { get; set; }

        public int TotalPrice { get; set; }

        public int DepositPercent { get; set; }

        public int PrepayAmount { get; set; }

        public DateTime HireDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public List<int> SlotsIdList { get; set; }

    }
}
