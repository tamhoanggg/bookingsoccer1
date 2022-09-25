using Firebase.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Security.Claims;

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
            //    ("testemail@gmail.com", "test123", "Tester");
            FirebaseAuthLink firebaseAuthLink =
                await firebaseAuthProvider.SignInWithEmailAndPasswordAsync("testemail@gmail.com", "test123");

            Console.WriteLine(firebaseAuthLink.FirebaseToken);

            IDataService dataService = RestService.For<IDataService>("http://localhost:5000");

            await dataService.GetData(firebaseAuthLink.FirebaseToken);

        }

    }

    public interface IDataService
    {
        [Get("/")]
        Task GetData([Authorize("Bearer")] string Token);
    }
}