namespace BarApplication.APIClient.Identity.Internal
{
    internal static class Constants
    {
        public const string LoginUrl = "api/Auth/Login";
        public const string RegisterUserUrl = "api/Auth/Register";
        public const string RefreshTokenUrl = "api/Auth/Refresh";
        public const string GetUserUrl = "/api/me";
        public const string Authorization = nameof(Authorization);
        public const string Bearer = nameof(Bearer);
    }
}