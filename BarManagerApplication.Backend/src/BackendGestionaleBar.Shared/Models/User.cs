using System;

namespace BackendGestionaleBar.Shared.Models
{
    public class User
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }
    }
}