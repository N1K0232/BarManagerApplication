using BackendGestionaleBar.DataAccessLayer.Entities.Common;

namespace BackendGestionaleBar.DataAccessLayer
{
    public interface IReadOnlyDataContext
    {
        IQueryable<T> GetData<T>(bool trackingChanges = false, bool ignoreQueryFilters = false) where T : BaseEntity;
        Task<T> GetAsync<T>(params object[] keyValues) where T : BaseEntity;
    }
}