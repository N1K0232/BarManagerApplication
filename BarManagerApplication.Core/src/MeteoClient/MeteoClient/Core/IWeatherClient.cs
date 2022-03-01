using System.Threading.Tasks;

namespace MeteoClient.Core
{
    public interface IWeatherClient
    {
        Task SearchAsync(string city);
    }
}