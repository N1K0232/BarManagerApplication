using Newtonsoft.Json;
using System.Text;

namespace MeteoClient.Models
{
    public class Location
    {
        [JsonConstructor]
        internal Location(string name, string region, string country)
        {
            Name = name;
            Region = region;
            Country = country;
        }

        public string Name { get; }
        public string Region { get; }
        public string Country { get; }

        public override string ToString()
        {
            StringBuilder builder = new();
            builder.AppendLine($"Name: {Name}");
            builder.AppendLine($"Region: {Region}");
            builder.AppendLine($"Country: {Country}");
            return builder.ToString();
        }
    }
}