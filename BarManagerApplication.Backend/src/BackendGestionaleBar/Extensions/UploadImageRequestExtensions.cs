using BackendGestionaleBar.BusinessLayer.Models;
using BackendGestionaleBar.Models;

namespace BackendGestionaleBar.Extensions;

public static class UploadImageRequestExtensions
{
    public static StreamFileContent ToStreamFileContent(this UploadImageRequest request)
    {
        return new StreamFileContent
        {
            Content = request.File.OpenReadStream(),
            ContentType = request.File.ContentType,
            FileName = request.File.FileName,
            Length = request.File.Length,
            Description = request.Description
        };
    }
}