using System.Collections.Generic;

namespace BarApplication.APIClient.Identity.Models.Responses
{
    public class RegisterResponse
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}