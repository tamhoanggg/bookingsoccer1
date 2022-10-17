using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.SoccerFieldInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSoccers.Service.Models.DTO.Payment;

namespace BookingSoccers.Service.Models.DTO.Booking
{
    public class AddedBookingView
    {
        public string UserName { get; set; }

        public string ZoneTypeName { get; set; }

        public int ZoneNumber { get; set; }

        public string FieldName { get; set; }

        public string FieldAddress { get; set; }

        public int TotalPrice { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        
        public DateTime CreateTime { get; set; }
  
        public StatusEnum Status { get; set; }

        public int DepositPercent { get; set; }

        public int DepositAmount { get; set; }

        public DateTime PayCreateTime { get; set; }

    }
}
