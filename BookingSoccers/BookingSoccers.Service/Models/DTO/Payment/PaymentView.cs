using BookingSoccers.Repo.Entities.BookingInfo;
using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Service.Models.DTO.Booking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.DTO.Payment
{
    public class PaymentView
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public GenderEnum Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public PaymentTypeEnum Type { get; set; }

        public int Amount { get; set; }

        public DateTime Time { get; set; }
    }
}
