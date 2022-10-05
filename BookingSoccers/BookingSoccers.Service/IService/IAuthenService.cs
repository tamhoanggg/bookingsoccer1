using BookingSoccers.Service.Models.Common;
using BookingSoccers.Service.Models.DTO.User;
using BookingSoccers.Service.Models.Payload;
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
        public Task<GeneralResult<LoginUserInfo>> Authentication(string IdToken);

        public Task<GeneralResult<APIToken>> RefreshToken(TokenRefresh tokenInfo);
    }
}
