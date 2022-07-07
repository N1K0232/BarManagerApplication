using BackendGestionaleBar.Shared.Common;

namespace BackendGestionaleBar.Shared.Requests;

public class SaveUmbrellaRequest : BaseRequestObject
{
    public int Row { get; set; }

    public string Letter { get; set; }
}