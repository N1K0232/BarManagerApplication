using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using System.Linq;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    public interface IDataContext
    {
        void Create<T>(T entity) where T : BaseEntity;
        void Delete<T>(T entity) where T : BaseEntity;
        Task<T> ReadAsync<T>(params object[] keyValues) where T : BaseEntity;
        IQueryable<T> GetData<T>(bool trackingChanges = false) where T : BaseEntity;
        Task SaveAsync();
    }
}