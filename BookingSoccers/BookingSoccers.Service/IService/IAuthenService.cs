using BookingSoccers.Service.DTO.UserInfo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookingSoccers.Service.IService
{
    public interface IAuthenService
    {
        public Task<LoginUserInfo> Authentication(string IdToken);

        public Task<IActionResult> RefreshToken(string AccessToken, string RefreshToken,
            DateTime refreshTokenExpiryDate);
    }
}
