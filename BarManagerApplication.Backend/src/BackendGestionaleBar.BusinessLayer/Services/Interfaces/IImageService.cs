using BackendGestionaleBar.BusinessLayer.Models;
using BackendGestionaleBar.Shared.Models;

namespace BackendGestionaleBar.BusinessLayer.Services.Interfaces;

public interface IImageService
{
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Image>> GetAsync(string path);
    Task<ImageStream> GetAsync(Guid id);
    Task<Image> UploadAsync(StreamFileContent content);
}