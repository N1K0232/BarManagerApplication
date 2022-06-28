using BackendGestionaleBar.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BackendGestionaleBar.Models;

public class UploadImageRequest
{
    [BindRequired]
    [AllowedExtensions("jpeg", "*.jpg", "*.png")]
    public IFormFile File { get; set; }

    public string Description { get; set; }
}