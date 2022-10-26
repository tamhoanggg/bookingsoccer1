using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload.User
{
    public class UserPredicate
    {
        public int? RoleId { get; set; }

        public int? GenderNum { get; set; }
    }
}
