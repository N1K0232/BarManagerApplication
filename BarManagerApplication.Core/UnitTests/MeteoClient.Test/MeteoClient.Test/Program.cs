using MeteoClient.Core;
using MeteoClient.Models;
using System.Threading.Tasks;

namespace MeteoClient.Test
{
    public class Program
    {
        public async static Task Main()
        {
            using var client = new WeatherClient();
            var request = new Request { City = "Rimini" };
            var response = await client.SearchAsync(request);
        }
    }
}