using BookingSoccers.Service.Models.DTO.User;

namespace BookingSoccers.Service.Models.DTO.User
{
    public class LoginUserInfo: RefreshTokenInfo
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public string JwtToken { get; set; }

        public DateTime ValidFromDate { get; set; }

        public DateTime ValidToDate { get; set; }
    }
}
