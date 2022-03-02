using System.Text;

namespace MeteoClient.Models
{
    public class Information
    {
        internal Information(string text, string icon, double? temperature)
        {
            Text = text;
            Icon = icon;
            Temperature = temperature;
        }

        public string Text { get; }
        public string Icon { get; }
        public double? Temperature { get; }

        public override string ToString()
        {
            StringBuilder builder = new();
            builder.AppendLine($"Text: {Text}");
            builder.AppendLine($"Icon: {Icon}");
            builder.AppendLine($"Temperature: {Temperature.Value}");
            return builder.ToString();
        }
    }
}