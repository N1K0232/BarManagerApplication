namespace BarApplication.APIClient.Identity.Internal
{
    internal static class Constants
    {
        public const string LoginUrl = "api/Auth/Login";
        public const string RegisterClienteUrl = "api/Auth/RegisterCliente";
        public const string RegisterStaffUrl = "api/Auth/RegisterStaff";
        public const string RefreshTokenUrl = "api/Auth/Refresh";
        public const string UpdatePasswordUrl = "api/Auth/UpdatePassword";
        public const string GetUserUrl = "/api/Me/GetMe";
        public const string Authorization = nameof(Authorization);
        public const string Bearer = nameof(Bearer);
    }
}