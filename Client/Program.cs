using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

            var TokenCLient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var TokenResponse = await TokenCLient.RequestResourceOwnerPasswordAsync("erick","password", "api1");

            if (TokenResponse.IsError)
            {
                Console.WriteLine(TokenResponse.Error);
                return;
            }

            Console.WriteLine(TokenResponse.Json);

            var client = new HttpClient();
            client.SetBearerToken(TokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5001/identity");

            if(!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        
    }
    }
}