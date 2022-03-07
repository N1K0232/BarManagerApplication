namespace MeteoClient.Models
{
    public class Response
    {
        internal Response(Information information, Location location)
        {
            Information = information;
            Location = location;
        }

        public Information Information { get; }
        public Location Location { get; }
    }
}