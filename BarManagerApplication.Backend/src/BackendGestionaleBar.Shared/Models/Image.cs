using BackendGestionaleBar.Shared.Common;

namespace BackendGestionaleBar.Shared.Models;

public class Image : BaseObject
{
    public string Path { get; set; }

    public int Length { get; set; }

    public string ContentType { get; set; }
}