using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SSL_Checker
{
    internal class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            await ProcessRepositories();
        }

        private static async Task ProcessRepositories()
        {
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
            };

            try
            {
                Console.WriteLine("Trying with system defaults");
                Console.WriteLine("-----");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
                var stringTask = await client.GetStringAsync("https://kamori.goats.dev/Plugin/CoreChangelog");

                dynamic msg = JsonConvert.DeserializeObject(stringTask);
                Console.WriteLine(msg[0].version);
                Console.WriteLine("-----");
                Console.WriteLine();
            }
            catch
            {
                Console.WriteLine("system defaults failed.");
            }

            try
            {
                Console.WriteLine("Trying with the same defaults as Dalamud (TLSv1.0-1.2");
                Console.WriteLine("-----");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls;
                var stringTask = await client.GetStringAsync("https://kamori.goats.dev/Plugin/CoreChangelog");

                dynamic msg = JsonConvert.DeserializeObject(stringTask);
                Console.WriteLine(msg[0].version);
                Console.WriteLine("-----");
                Console.WriteLine();
            }
            catch
            {
                Console.WriteLine("TLSv1.0-1.2 failed.");
            }

            try
            {
                Console.WriteLine("Trying with only TLSv1.2");
                Console.WriteLine("-----");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var stringTask = await client.GetStringAsync("https://kamori.goats.dev/Plugin/CoreChangelog");

                // var msg = stringTask;
                dynamic msg = JsonConvert.DeserializeObject(stringTask);


                Console.WriteLine(msg[0].version);
                Console.WriteLine("-----");
                Console.WriteLine();
            }
            catch
            {
                Console.WriteLine("TLSv1.2 only failed.");
            }

            Console.Write("Press the return key to exit.");
            Console.Read();
        }
    }


}
