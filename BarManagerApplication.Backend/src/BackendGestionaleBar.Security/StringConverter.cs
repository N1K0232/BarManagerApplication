using System.Text;

namespace BackendGestionaleBar.Security;

public static class StringConverter
{
    public static string GetString(string encodedString)
    {
        byte[] bytes = Convert.FromBase64String(encodedString);
        string decodedString = Encoding.UTF8.GetString(bytes);
        return decodedString;
    }
}