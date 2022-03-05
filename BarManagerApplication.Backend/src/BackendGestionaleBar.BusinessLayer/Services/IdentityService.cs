using BackendGestionaleBar.DataAccessLayer;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public class IdentityService
    {
        private readonly IDatabase db;

        public IdentityService(IDatabase db)
        {
            this.db = db;
        }
    }
}