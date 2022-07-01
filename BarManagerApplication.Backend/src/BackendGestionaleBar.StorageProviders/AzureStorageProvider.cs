using BackendGestionaleBar.StorageProviders.Common;
using BackendGestionaleBar.StorageProviders.Settings;

namespace BackendGestionaleBar.StorageProviders;

internal class AzureStorageProvider : IStorageProvider
{
    private readonly AzureStorageSettings settings;

    public AzureStorageProvider(AzureStorageSettings settings)
    {
        this.settings = settings;
    }

    public Task SaveAsync(string path, Stream stream)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> ReadAsync(string path)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string path)
    {
        throw new NotImplementedException();
    }
}