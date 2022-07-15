using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendGestionaleBar.BusinessLayer.Services.Interfaces;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Requests;
using Microsoft.EntityFrameworkCore;
using Entities = BackendGestionaleBar.DataAccessLayer.Entities;

namespace BackendGestionaleBar.BusinessLayer.Services;

public sealed class UmbrellaService : IUmbrellaService
{
    private readonly IDataContext dataContext;
    private readonly IMapper mapper;

    public UmbrellaService(IDataContext dataContext, IMapper mapper)
    {
        this.dataContext = dataContext;
        this.mapper = mapper;
    }

    public async Task DeleteAsync(Guid id)
    {
        var dbUmbrella = await dataContext.GetAsync<Entities.Umbrella>(id);
        dataContext.Delete(dbUmbrella);
        await dataContext.SaveAsync();
    }

    public async Task<IEnumerable<Umbrella>> GetAsync(string coordinates)
    {
        var query = dataContext.GetData<Entities.Umbrella>();

        if (!string.IsNullOrWhiteSpace(coordinates))
        {
            query = query.Where(u => u.Coordinates.Equals(coordinates));
        }

        var umbrellas = await query.ProjectTo<Umbrella>(mapper.ConfigurationProvider).ToListAsync();
        return umbrellas;
    }

    public async Task<Umbrella> SaveAsync(SaveUmbrellaRequest request)
    {
        var query = dataContext.GetData<Entities.Umbrella>(trackingChanges: true);
        var dbUmbrella = request.Id != null ? await query.FirstOrDefaultAsync(u => u.Id == request.Id) : null;

        if (dbUmbrella == null)
        {
            dbUmbrella = new Entities.Umbrella
            {
                Coordinates = $"{request.Letter}{request.Number}"
            };

            dataContext.Insert(dbUmbrella);
        }
        else
        {
            dbUmbrella.Coordinates = $"{request.Letter}{request.Number}";
            dataContext.Edit(dbUmbrella);
        }

        await dataContext.SaveAsync();
        return mapper.Map<Umbrella>(dbUmbrella);
    }
}