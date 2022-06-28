using BackendGestionaleBar.StorageProviders.Common;
using BackendGestionaleBar.StorageProviders.Settings;

namespace BackendGestionaleBar.StorageProviders;

internal class FileSystemStorageProvider : IStorageProvider
{
    private readonly FileSystemStorageSettings settings;

    public FileSystemStorageProvider(FileSystemStorageSettings settings)
    {
        this.settings = settings;
    }

    public async Task SaveAsync(string path, Stream stream)
    {
        string fullPath = GetFullPath(path);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        using var outputStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);

        stream.Position = 0;
        await stream.CopyToAsync(outputStream);

        outputStream.Close();
    }

    public Task<Stream> ReadAsync(string path)
    {
        string fullPath = GetFullPath(path);
        if (!File.Exists(fullPath))
        {
            return null;
        }

        FileStream stream = File.OpenRead(fullPath);
        return Task.FromResult<Stream>(stream);
    }

    public Task DeleteAsync(string path)
    {
        string fullPath = GetFullPath(path);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    private string GetFullPath(string path) => Path.Combine(settings.StorageFolder, path);
}