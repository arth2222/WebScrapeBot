using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapeBot
{

    public class Person
    {
        public int Id { get; set; }
        public string Navn { get; set; }

        void KjøreBil()
        {

        }
    }
    internal class Program
    {
        


        static int counter = 0;
        static async Task Main(string[] args)
        {


            //long num = DateTime.Now.Ticks;
            var ipAddresses = GetAllPossibleIPAddresses();
            //foreach (string ip in ipAddresses)
            //    MakeWebRequest(ip);



            //string content=MakeWebRequest("195.88.55.16");



            //foreach (var ipAddress in ipAddresses)
            //{
            //    //MakeWebRequest(ipAddress);
            //    Console.WriteLine(ipAddress);

            //}

            var httpClientHandler = new HttpClientHandler
            {
                UseProxy = false,
                UseDefaultCredentials = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var httpClient = new HttpClient(httpClientHandler);
            httpClient.Timeout = TimeSpan.FromSeconds(5);

            var tasks = new List<Task>();

            foreach (var ipAddress in ipAddresses)
            {
                tasks.Add(CheckIpAddress(httpClient, ipAddress));
            }
            await Task.WhenAll(tasks);
        }

        private List<string> GenerateIPAddresssesList()
        {
            List<string> list = new List<string>();
            for (int a = 0; a < 256; a++)
            {
                for (int b = 0; b < 256; b++)
                {
                    for (int c = 0; c < 256; c++)
                    {
                        for (int d = 0; d < 256; d++)
                        {
                            list.Add(a + "." + b + "." + c + "." + d);
                        }
                    }
                }
            }
            return list;
        }

        public static string MakeWebRequest(string ipAddress)
        {
            try
            {
                string url = $"http://{ipAddress}";
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 5000;
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, for example by logging it or displaying an error message
                Console.WriteLine($"An exception occurred: {ex.Message}");
                return null;
            }
        }



        static IEnumerable<string> GetAllPossibleIPAddresses()
        {
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    for (int k = 0; k < 256; k++)
                    {
                        for (int l = 0; l < 256; l++)
                        {
                            yield return $"{i}.{j}.{k}.{l}";
                        }
                    }
                }
            }
        }

        static async Task CheckIpAddress(HttpClient client, string ipAddress)
        {
            try
            {
                var response = await client.GetAsync($"http://{ipAddress}");
                Console.WriteLine(counter);
                counter++;

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Website exists. " + ipAddress);
                }
                else
                {
                    Console.WriteLine("Website does not exist. " + ipAddress);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(counter);
                counter++;
                Console.WriteLine($"An HTTP error occurred: {ex.Message}" + ipAddress);
            }
        }
    }
}

//Using async and await allows you to write asynchronous code that runs more efficiently and provides a better user experience.

//When you make an HTTP request using HttpClient, the call to GetAsync method sends the request to the server and waits for the server to send back a response. This can take a significant amount of time, especially if the server is slow or the response is large. If your application were to block and wait for the response to come back, it would become unresponsive and appear to hang.

//By using async and await, your code can continue to execute while the request is being sent and the response is being received. This allows your application to remain responsive and handle other tasks while the request is in progress. When the response is finally received, the await keyword will pause execution of the method until the response is available, and then continue executing the method with the response data.

//In addition to improving performance and responsiveness, using async and await can also make your code easier to read and maintain, since it allows you to write asynchronous code in a way that looks similar to synchronous code.