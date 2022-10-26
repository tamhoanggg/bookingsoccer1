using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.DTO
{
    public class ObjectListPagingInfo
    {
        public int CurrentPage { get; set; }

        public int TotalElement { get; set; }

        public int TotalPage { get; set; }

        public object? ObjectList { get; set; }
    }
}
