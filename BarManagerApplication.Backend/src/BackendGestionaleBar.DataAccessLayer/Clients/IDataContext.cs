using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    public interface IDataContext
    {
        void Delete<T>(T entity) where T : class;
        ValueTask<T> GetAsync<T>(params object[] keyValues) where T : class;
        void Insert<T>(T entity) where T : class;
        void Edit<T>(T entity) where T : class;

        Task<bool> SaveAsync();
    }
}