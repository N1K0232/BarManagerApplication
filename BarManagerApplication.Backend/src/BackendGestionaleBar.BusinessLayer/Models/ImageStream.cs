namespace BackendGestionaleBar.BusinessLayer.Models;

public class ImageStream
{
    public ImageStream(Stream stream, string contentType)
    {
        Stream = stream;
        ContentType = contentType;
    }

    public Stream Stream { get; }
    public string ContentType { get; }
}