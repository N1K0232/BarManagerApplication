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
        try
        {
            string fullPath = GetFullPath(path);
            string directoryName = Path.GetDirectoryName(fullPath);
            bool directoryExists = Directory.Exists(directoryName);

            if (!directoryExists)
            {
                Directory.CreateDirectory(directoryName);
            }

            using var outputStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);

            stream.Position = 0;
            await stream.CopyToAsync(outputStream).ConfigureAwait(false);

            outputStream.Close();
        }
        catch (Exception ex)
        {
            await Task.FromException(ex);
        }
    }

    public Task<Stream> ReadAsync(string path)
    {
        FileStream stream;

        try
        {
            string fullPath = GetFullPath(path);
            bool fileExists = File.Exists(fullPath);
            if (!fileExists)
            {
                stream = null;
            }

            stream = File.OpenRead(fullPath);
        }
        catch (Exception)
        {
            stream = null;
        }

        return Task.FromResult<Stream>(stream);
    }

    public Task DeleteAsync(string path)
    {
        Task result;

        try
        {
            string fullPath = GetFullPath(path);
            bool fileExists = File.Exists(fullPath);
            if (fileExists)
            {
                File.Delete(fullPath);
            }

            result = Task.CompletedTask;
        }
        catch (Exception ex)
        {
            result = Task.FromException(ex);
        }

        return result;
    }

    private string GetFullPath(string path)
    {
        return Path.Combine(settings.StorageFolder, path);
    }
}