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
        private const string BaseUrl = "http://api.weatherapi.com/v1/current.json?key=";
        private const string ApiKey = "7f4389ff11a94fd4938110520212306";

        private string url = "";
        private string result;

        private Information _info = null;
        private Location _location = null;

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

        public Information Info => _info;
        public Location Location => _location;

        public async Task SearchAsync(string city)
        {
            string text;
            string icon;
            double? temperature;

            using var client = new HttpClient();
            url += $"&q={city}&aqi=no";

            try
            {
                result = await client.GetStringAsync(url);
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
                return;
            }

            var deserialized = JObject.Parse(result);
            try
            {
                temperature = deserialized["current"]?["temp_c"]?.ToObject<double?>();
                text = deserialized["current"]?["condition"]?["text"]?.ToString();
                icon = deserialized["current"]?["condition"]?["icon"]?.ToString();
                _location = deserialized["location"]?.ToObject<Location>();
                _info = new Information(text, icon, temperature);
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
        }
    }
}