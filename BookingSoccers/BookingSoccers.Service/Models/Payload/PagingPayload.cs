﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload
{
    public class PagingPayload
    {
        [Required]
        public int PageNum { get; set; }

        [Required]
        public string OrderColumn { get; set; } = "Id";

        [Required]
        public bool IsAscending { get; set; } = true;

    }
}
