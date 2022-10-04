namespace BookingSoccers.Service.Models.DTO.User
{
    public class RefreshTokenInfo
    {
        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpirationTime { get; set; }
    }
}
