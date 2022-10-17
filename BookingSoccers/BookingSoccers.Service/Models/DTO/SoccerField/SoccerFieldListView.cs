using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.DTO.SoccerField
{
    public class SoccerFieldListView
    {
        public int Id { get; set; }

        public string FieldName { get; set; }

        public string ManagerPhoneNumber { get; set; }

        public string Address { get; set; }
    }
}
