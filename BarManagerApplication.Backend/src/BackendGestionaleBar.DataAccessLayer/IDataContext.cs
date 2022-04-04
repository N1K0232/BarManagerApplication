using BackendGestionaleBar.DataAccessLayer.Entities.Common;

namespace BackendGestionaleBar.DataAccessLayer
{
    public interface IDataContext : IReadOnlyDataContext
    {
        void Delete<T>(T entity) where T : BaseEntity;
        void Delete<T>(IEnumerable<T> entities) where T : BaseEntity;
        void Insert<T>(T entity) where T : BaseEntity;

        Task SaveAsync();
    }
}