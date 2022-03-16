using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer.Clients
{
    public interface IDataContext
    {
        Task AddAsync<T>(T entity) where T : class;
        Task DeleteAsync<T>(T entity) where T : class;
        ValueTask<T> GetAsync<T>(params object[] keyValues) where T : class;
        Task UpdateAsync<T>(T entity) where T : class;
    }
}