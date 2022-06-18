using AutoMapper;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Entities = BackendGestionaleBar.DataAccessLayer.Entities;

namespace BackendGestionaleBar.BusinessLayer.Services;

public class ProductService : IProductService
{
	private readonly IDataContext dataContext;
	private readonly IMapper mapper;

	public ProductService(IDataContext dataContext, IMapper mapper)
	{
		this.dataContext = dataContext;
		this.mapper = mapper;
	}

	public async Task<Product> SaveAsync(SaveProductRequest request)
	{
		var query = dataContext.GetData<Entities.Product>(trackingChanges: true);
		var dbProduct = request.Id != null ? await query.FirstOrDefaultAsync(p => p.Id == request.Id) : null;

		if (dbProduct == null)
		{
			dbProduct = mapper.Map<Entities.Product>(request);
			dbProduct.Category = await dataContext.GetData<Entities.Category>().FirstOrDefaultAsync(c => c.Name == request.CategoryName);
			dataContext.Insert(dbProduct);
		}
		else
		{
			mapper.Map(request, dbProduct);
			dbProduct.Category = await dataContext.GetData<Entities.Category>().FirstOrDefaultAsync(c => c.Name == request.CategoryName);
			dataContext.Edit(dbProduct);
		}

		await dataContext.SaveAsync();

		return mapper.Map<Product>(dbProduct);
	}
}