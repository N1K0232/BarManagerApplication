using System;
using System.Text;

namespace BackendGestionaleBar.Helpers
{
    public static class StringConverter
    {
        public static string GetString(string hash)
        {
            byte[] bytes = Convert.FromBase64String(hash);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}