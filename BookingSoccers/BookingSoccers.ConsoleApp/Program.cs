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
        private const string API_KEY = "AIzaSyCSYPWwr8YTJ3_vYAynxeZr37OKSC9VQng";

        static async Task Main(string[] args) 
        {
            FirebaseAuthProvider firebaseAuthProvider = 
                new FirebaseAuthProvider(new FirebaseConfig(API_KEY));

            //FirebaseAuthLink firebaseAuthLink =
            //    await firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync
            //    ("maithanhhoang.08032001@gmail.com", "hoang123", "MTHoang");

            FirebaseAuthLink firebaseAuthLink =
                await firebaseAuthProvider.SignInWithEmailAndPasswordAsync(
                    "maithanhhoang.08032001@gmail.com", "hoang123");

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