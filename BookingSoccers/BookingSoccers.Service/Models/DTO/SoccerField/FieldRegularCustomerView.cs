using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.DTO.SoccerField
{
    public class FieldRegularCustomerView
    {
        public string UserName { get; set; }

        public string Phone { get; set; }

        public int BookingCount { get; set; }
    }
}
