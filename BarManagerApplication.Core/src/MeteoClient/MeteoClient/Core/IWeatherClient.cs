using MeteoClient.Models;
using System;
using System.Threading.Tasks;

namespace MeteoClient.Core
{
    public interface IWeatherClient : IDisposable
    {
        Task<Response> SearchAsync(Request request);
    }
}