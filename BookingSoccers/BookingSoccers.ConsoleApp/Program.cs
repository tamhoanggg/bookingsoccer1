using Firebase.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Refit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BookingSoccers.ConsoleApp
{
    public class Program
    {
        //private const string API_KEY = "AIzaSyCSYPWwr8YTJ3_vYAynxeZr37OKSC9VQng";
        private const string API_KEY = "AIzaSyCxl4kbzsuDoDDJvz8In5fFQDHww97qr_s";
        static async Task Main(string[] args) 
        {
            FirebaseAuthProvider firebaseAuthProvider = 
                new FirebaseAuthProvider(new FirebaseConfig(API_KEY));

            //FirebaseAuthLink firebaseAuthLink =
                //await firebaseAuthProvider.SignInWithEmailAndPasswordAsync
                //("hoang.nqm45@gmail.com", "123456");

            //FirebaseAuthLink firebaseAuthLink =
                //await firebaseAuthProvider.SignInWithEmailAndPasswordAsync
                //("zidanedautroc@gmail.com", "123456");

            //FirebaseAuthLink firebaseAuthLink =
             // await firebaseAuthProvider.SignInWithEmailAndPasswordAsync(
                    //"maithanhhoang.08032001@gmail.com", "hoang123");

            FirebaseAuthLink firebaseAuthLink =
             await firebaseAuthProvider.SignInWithGoogleIdTokenAsync("eyJhbGciOiJSUzI1NiIsImtpZCI6IjU4NWI5MGI1OWM2YjM2ZDNjOTBkZjBlOTEwNDQ1M2U2MmY4ODdmNzciLCJ0eXAiOiJKV1QifQ.eyJuYW1lIjoicXVhbmcgbmd1eWVuIiwicGljdHVyZSI6Imh0dHBzOi8vbGgzLmdvb2dsZXVzZXJjb250ZW50LmNvbS9hL0FMbTV3dTItVWo4dGtrZzJTd25VMS0zbzh3b0xTa3o5ZktqWkY1ek5GY1lmPXM5Ni1jIiwiaXNzIjoiaHR0cHM6Ly9zZWN1cmV0b2tlbi5nb29nbGUuY29tL2Jvb2tpbmdzb2NjZXJmaWVsZCIsImF1ZCI6ImJvb2tpbmdzb2NjZXJmaWVsZCIsImF1dGhfdGltZSI6MTY2NTAxODIzNiwidXNlcl9pZCI6InhLS2tnNmNjY01YeHJPMkhmRTRDbWRVR2YwOTMiLCJzdWIiOiJ4S0trZzZjY2NNWHhyTzJIZkU0Q21kVUdmMDkzIiwiaWF0IjoxNjY1MDE4MjM2LCJleHAiOjE2NjUwMjE4MzYsImVtYWlsIjoiemlkYW5lZGF1dHJvY0BnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJnb29nbGUuY29tIjpbIjExMzc0NjM1MjA5NTI2NzE0NDcwOCJdLCJlbWFpbCI6WyJ6aWRhbmVkYXV0cm9jQGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6Imdvb2dsZS5jb20ifX0.HSEvr - WoiTxgGtoZIQSp5zJ4TX - cL8Gs1JqJPnyzQZx06 - hMhww5kMNnioLjp7ERdpgNneU6DuGapwblmhaxu9fnc8r2ruSKzqMFvDir6mFC - 82wdcZ0PB_Ni2VgS8i_9Tax5MQX7tf5WjSZVJJbl1f - CNumEHcu_LFRq6dmmIciN_1B2rZRlhnRIULut8v6wUivubSJOzYudI_lFaAfZke - 33IDlYsN6MLkbisgWN1kbORJkraOZJYIwBvgjFUOyf5z1Ue2j1LZ7e2uRhrNoRTNBDx9qCuwOQ7ZhTI16bSnxXFYi6P29ZWtNOQFt8cPG4CiJo0QTXtn1a9igXsUow");

            Console.WriteLine(firebaseAuthLink.FirebaseToken);

            IDataService dataService = RestService.For<IDataService>("http://localhost:5000");

            string actionResult =  await dataService.Login(firebaseAuthLink.FirebaseToken);

            string EmailVal = ValidateToken(actionResult)?.FindFirst("email").Value;

            string IdVal = ValidateToken(actionResult)?.FindFirst("id")?.Value;

            string NameVal = ValidateToken(actionResult)?.FindFirst("name")?.Value;

            Console.WriteLine(IdVal);
            Console.WriteLine();

            Console.WriteLine(EmailVal);
            Console.WriteLine();

            Console.WriteLine(NameVal);
            Console.WriteLine();

            //Console.WriteLine("Fuck you before");

        }

        public static ClaimsPrincipal ValidateToken(string Token) 
        {
            var _issuer = "http://localhost:5000";
            var _audience = "http://localhost:5000";
            //var _audiences = "https://hoangmt.com/";
            var _secretKey = "This is a sample secret key - please don't use in production environment.'";
            JwtSecurityTokenHandler SecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenHandler.InboundClaimTypeMap.Clear();
            SecurityTokenHandler.OutboundClaimTypeMap.Clear();
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.ValidateLifetime = true;
            validationParameters.ValidIssuer = _issuer.ToLower();
            validationParameters.ValidAudience = _audience.ToLower();
            //validationParameters.ValidAudiences = _audiences.ToLower();
            validationParameters.IssuerSigningKey =
                new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_secretKey));

            ClaimsPrincipal principal = SecurityTokenHandler.ValidateToken(
                Token, validationParameters,out validatedToken);
            return principal;
        }

    }

    public interface IDataService
    {
        [Get("/login")]
        Task<String> Login([Authorize("Bearer")] string Token);
    }
}