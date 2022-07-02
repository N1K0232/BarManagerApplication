using System.Text;

namespace BackendGestionaleBar.Security;

public static class StringConverter
{
    public static string GetString(string decodedString)
    {
        byte[] bytes = Convert.FromBase64String(decodedString);
        return Encoding.UTF8.GetString(bytes);
    }
}