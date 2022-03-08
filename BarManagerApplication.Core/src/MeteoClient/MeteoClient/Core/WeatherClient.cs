using MeteoClient.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;

namespace MeteoClient.Core
{
    public sealed class WeatherClient : IWeatherClient
    {
        private string url = "";
        private string result = null;

        private string text = "";
        private string icon = "";
        private double? temperature = 0;

        private readonly HttpClient httpClient = new();

        public WeatherClient()
        {
            try
            {
                url = $"{BaseUrl}{ApiKey}";
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (SecurityException ex)
            {
                throw ex;
            }
        }

        private string BaseUrl
        {
            get
            {
                string key = nameof(BaseUrl);
                return Environment.GetEnvironmentVariable(key);
            }
        }
        private string ApiKey
        {
            get
            {
                string key = nameof(ApiKey);
                return Environment.GetEnvironmentVariable(key);
            }
        }

        public async Task<Response> SearchAsync(Request request)
        {
            Location location;
            Information information;

            url += $"&q={request.City}&aqi=no";

            try
            {
                result = await httpClient.GetStringAsync(url);
                text = "";
            }
            catch (HttpRequestException)
            {
                text = "Inserisci una città valida";
            }
            catch (InvalidOperationException)
            {
                text = "Error";
            }

            if (!string.IsNullOrEmpty(text))
            {
                return null;
            }

            JObject deserialized = JObject.Parse(result);
            try
            {
                temperature = deserialized["current"]?["temp_c"]?.ToObject<double?>();
                text = deserialized["current"]?["condition"]?["text"]?.ToString();
                icon = deserialized["current"]?["condition"]?["icon"]?.ToString();
                location = deserialized["location"]?.ToObject<Location>();
                information = new(text, icon, temperature);
            }
            catch (NullReferenceException)
            {
                location = null;
                information = null;
            }
            catch (InvalidOperationException)
            {
                location = null;
                information = null;
            }

            if (location == null && information == null)
            {
                return null;
            }
            else
            {
                return new Response(information, location);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && httpClient != null)
            {
                httpClient.Dispose();
            }
        }
    }
}