namespace BackendGestionaleBar.StorageProviders.Common;

internal class BlobContainer
{
    internal BlobContainer(string containerName, string blobName)
    {
        ContainerName = containerName;
        BlobName = blobName;
    }

    public string ContainerName { get; }

    public string BlobName { get; }
}