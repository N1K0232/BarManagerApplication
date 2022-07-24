using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendGestionaleBar.BusinessLayer.Services.Interfaces;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Requests;
using Microsoft.EntityFrameworkCore;
using TinyHelpers.Extensions;
using Entities = BackendGestionaleBar.DataAccessLayer.Entities;

namespace BackendGestionaleBar.BusinessLayer.Services;

public sealed class CategoryService : ICategoryService
{
	private readonly IDataContext dataContext;
	private readonly IMapper mapper;

	public CategoryService(IDataContext dataContext, IMapper mapper)
	{
		this.dataContext = dataContext;
		this.mapper = mapper;
	}

	public async Task DeleteAsync(Guid id)
	{
		var dbCategory = await dataContext.GetAsync<Entities.Category>(id);
		dataContext.Delete(dbCategory);
		await dataContext.SaveAsync();
	}

	public async Task<IEnumerable<Category>> GetAsync(string name)
	{
		var query = dataContext.GetData<Entities.Category>();

		if (name.HasValue())
		{
			query = query.Where(c => c.Name.Contains(name));
		}

		var categories = await query.OrderBy(c => c.Name)
			.ProjectTo<Category>(mapper.ConfigurationProvider)
			.ToListAsync();

		return categories;
	}

	public async Task<Category> SaveAsync(SaveCategoryRequest request)
	{
		var query = dataContext.GetData<Entities.Category>(trackingChanges: true);
		var dbCategory = request.Id != null ? await query.FirstOrDefaultAsync(c => c.Id == request.Id) : null;

		if (dbCategory == null)
		{
			dbCategory = mapper.Map<Entities.Category>(request);
			dataContext.Insert(dbCategory);
		}
		else
		{
			mapper.Map(request, dbCategory);
			dataContext.Edit(dbCategory);
		}

		await dataContext.SaveAsync();

		return mapper.Map<Category>(dbCategory);
	}
}