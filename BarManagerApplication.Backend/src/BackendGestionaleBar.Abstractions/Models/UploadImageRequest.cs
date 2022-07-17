using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BackendGestionaleBar.Abstractions.Models;

public class UploadImageRequest
{
    [BindRequired]
    [AllowedExtensions("jpeg", "*.jpg", "*.png")]
    public IFormFile File { get; set; } = null!;

    public string? Description { get; set; }
}