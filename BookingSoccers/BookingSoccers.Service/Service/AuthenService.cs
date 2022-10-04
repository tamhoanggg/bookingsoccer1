//using BookingSoccers.Repo.Entities.UserInfo;
using BookingSoccers.Repo.IRepository.UserInfo;
using BookingSoccers.Service.Models.DTO.User;
using BookingSoccers.Service.IService;
using Firebase.Auth;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BookingSoccers.Service.Models.Common;
using Microsoft.EntityFrameworkCore;
using BookingSoccers.Service.Models.Payload;
//using Firebase.Auth;
//using BookingSoccers.Service.Service.UserInfo;

namespace BookingSoccers.Service.Service
{
    public class AuthenService : IAuthenService
    {
        private IUserRepo userRepo;
        private string secret_key = "This is a sample secret key - please don't use in production environment.'";
        private string issuer = "http://localhost:5000";
        private string audience = "http://localhost:5000";
        private int tokenValidityInHours = 6;
        private int refreshTokenValidityInDays = 15;

        public AuthenService(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret_key)),
                ValidateLifetime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                StringComparison.InvariantCultureIgnoreCase) || 
                securityToken.ValidTo > DateTime.UtcNow)
                throw new SecurityTokenException("Invalid token");

            return principal;

        }

        public async Task<GeneralResult<LoginUserInfo>> Authentication(string IdToken)
        {

            string API_key = "AIzaSyCSYPWwr8YTJ3_vYAynxeZr37OKSC9VQng";
            string RoleName = "";
            string tokenString = "";
            string RefreshToken = "";

            LoginUserInfo userInfo = new LoginUserInfo();

            try
            {
                FirebaseToken decodedToken = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance
                            .VerifyIdTokenAsync(IdToken);
                string uid = decodedToken.Uid;
                var authUser = new FirebaseAuthProvider(new FirebaseConfig(API_key));
                var auth = authUser.GetUserAsync(IdToken);
                User user = auth.Result;
                var UserWithEmail = await userRepo.Get().Where(x =>
                x.Email == user.Email.ToLower()).FirstOrDefaultAsync();

                if (UserWithEmail == null) return null;

                userInfo.UserName = UserWithEmail.UserName;
                userInfo.Email = UserWithEmail.Email;


                if (UserWithEmail.RoleId == 1)
                {
                    userInfo.Role = "Admin";
                    RoleName = "Admin";
                }
                else if (UserWithEmail.RoleId == 2)
                {
                    userInfo.Role = "Manager";
                    RoleName = "Manager";
                }
                else if (UserWithEmail.RoleId == 3)
                {
                    userInfo.Role = "User";
                    RoleName = "User";
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, UserWithEmail.UserName),
                    new Claim(ClaimTypes.Email, UserWithEmail.Email),
                    new Claim(ClaimTypes.Role, RoleName),
                    new Claim(ClaimTypes.GivenName, uid)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secret_key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Issuer = issuer,
                    Audience = audience,
                    Expires = DateTime.UtcNow.AddHours(5),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                userInfo.ValidFromDate = token.ValidFrom;
                userInfo.ValidToDate = token.ValidTo;
                tokenString = tokenHandler.WriteToken(token);
                RefreshToken = GenerateRefreshToken();
            }
            catch (Exception e) 
            { 
                Console.WriteLine(e.Message);
            } 
            Console.WriteLine("Called get Data");
            var refreshTokenExiryTime = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);
            userInfo.JwtToken = tokenString;
            userInfo.RefreshToken = RefreshToken;
            userInfo.RefreshTokenExpirationTime = refreshTokenExiryTime;

            return GeneralResult<LoginUserInfo>.Success(userInfo);
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret_key));
          
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.Now.AddHours(tokenValidityInHours),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public async Task<GeneralResult<APIToken>> RefreshToken(TokenRefresh tokenInfo)
        {
            if (tokenInfo.AccessToken is null || tokenInfo.RefreshToken is null)
                return GeneralResult<APIToken>.Error(400,
                        "Access token or refresh token is empty");

            string? accessToken = tokenInfo.AccessToken;
            string? refreshToken = tokenInfo.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null) 
                GeneralResult<APIToken>.Error(400,"Access token is invalid");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string username = principal.Identity.Name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            var user = await userRepo.Get().Where(x =>
                 x.UserName == username).FirstOrDefaultAsync();

            if (user == null || tokenInfo.RefreshTokenExpiryDate <= DateTime.UtcNow)
                return GeneralResult<APIToken>.Error(400,
                    "Access token or refresh token is invalid");

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            return GeneralResult<APIToken>.Success(new APIToken()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            });
        }

    }
}
