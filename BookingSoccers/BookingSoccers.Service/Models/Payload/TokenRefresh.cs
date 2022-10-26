﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.Models.Payload
{
    public class TokenRefresh
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public DateTime RefreshTokenExpiryDate { get; set; }
    }
}
