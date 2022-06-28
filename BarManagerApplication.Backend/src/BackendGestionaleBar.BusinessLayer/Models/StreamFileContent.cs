namespace BackendGestionaleBar.BusinessLayer.Models;

public class StreamFileContent
{
    public Stream Content { get; set; }

    public string ContentType { get; set; }

    public string FileName { get; set; }

    public long Length { get; set; }

    public string Description { get; set; }
}