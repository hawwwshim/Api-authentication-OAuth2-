using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FirebaseAuthConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = ReadPassword();

            // Firebase Authentication REST API URL for sign-in
            string apiKey = "AIzaSyCnau0Ny4bVyKwvUdmxTBeDbkXzq4_2FEY"; // Replace YOUR_API_KEY with your Firebase Web API key
            string url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}";

            var requestPayload = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            using (HttpClient client = new HttpClient())
            {
                var json = JObject.FromObject(requestPayload).ToString();
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseData);
                    string idToken = jsonResponse["idToken"]?.ToString();
                    string refreshToken = jsonResponse["refreshToken"]?.ToString();

                    Console.WriteLine($"ID Token: {idToken}");
                    Console.WriteLine($"Refresh Token: {refreshToken}");
                }
                else
                {
                    Console.WriteLine($"Error: {response.ReasonPhrase}");
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error Details: {errorContent}");
                }
            }
        }
        private static string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Length--;
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    password.Append(keyInfo.KeyChar);
                    Console.Write("*");
                }
            }
            return password.ToString();
        }
    }
}
