using BackendGestionaleBar.Abstractions.Models;
using BackendGestionaleBar.BusinessLayer.Models;

namespace BackendGestionaleBar.Extensions;

public static class UploadImageRequestExtensions
{
    public static StreamFileContent ToStreamFileContent(this UploadImageRequest request)
    {
        var content = new StreamFileContent
        {
            Content = request.File.OpenReadStream(),
            ContentType = request.File.ContentType,
            FileName = request.File.FileName,
            Length = request.File.Length,
            Description = request.Description
        };

        return content;
    }
}