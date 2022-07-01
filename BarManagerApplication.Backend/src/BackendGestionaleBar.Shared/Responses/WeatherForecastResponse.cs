namespace BackendGestionaleBar.Shared.Responses;

public class WeatherForecastResponse
{
    public string Text { get; set; }

    public string Icon { get; set; }

    public double? Temperature { get; set; }

    public string Location { get; set; }

    public string Region { get; set; }

    public string Country { get; set; }
}