using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Common
{
    public class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public string Msg { get; set; }
    }
}
