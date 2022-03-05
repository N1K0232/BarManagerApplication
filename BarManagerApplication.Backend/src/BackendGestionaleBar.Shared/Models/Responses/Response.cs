using System.Collections.Generic;

namespace BackendGestionaleBar.Shared.Models.Responses
{
    public class Response
    {
        public bool Succeeded { get; set; }

        public List<string> Errors { get; set; }
    }
}