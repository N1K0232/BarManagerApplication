using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using BackendGestionaleBar.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace BackendGestionaleBar.BusinessLayer.Services.Common;

public abstract class BaseService
{
    private readonly IBarManagerDataContext dataContext;

    protected BaseService(IBarManagerDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    protected async Task<T> FindAsync<T>(Guid id) where T : BaseEntity
    {
        var entity = await dataContext.GetAsync<T>(id);
        return entity;
    }
    protected async Task<T> GetEntity<T>(BaseRequestObject request, bool trackingChanges = false, bool ignoreQueryFilters = false) where T : BaseEntity
    {
        var query = dataContext.GetData<T>(trackingChanges, ignoreQueryFilters);
        var entity = request.Id != null ? await query.FirstOrDefaultAsync(e => e.Id == request.Id) : null;
        return entity;
    }
}