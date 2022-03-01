using MeteoClient.Core;
using System;
using System.Threading.Tasks;

namespace MeteoClient.Test
{
    public class Program
    {
        public async static Task Main()
        {
            var client = new WeatherClient();
            await client.SearchAsync("Rimini");

            Console.Write(client.Location);
            Console.Write(client.Info);
        }
    }
}