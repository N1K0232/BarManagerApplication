using AutoMapper;
using BackendGestionaleBar.BusinessLayer.Services.Common;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Requests;
using Microsoft.EntityFrameworkCore;
using Entities = BackendGestionaleBar.DataAccessLayer.Entities;

namespace BackendGestionaleBar.BusinessLayer.Services;

public sealed class ProductService : IProductService
{
	private readonly IBarManagerDataContext dataContext;
	private readonly IMapper mapper;

	public ProductService(IBarManagerDataContext dataContext, IMapper mapper)
	{
		this.dataContext = dataContext;
		this.mapper = mapper;
	}

	public async Task DeleteAsync(Guid id)
	{
		var dbProduct = await dataContext.GetAsync<Entities.Product>(id);
		dataContext.Delete(dbProduct);
		await dataContext.SaveAsync();
	}

	public async Task<IEnumerable<Product>> GetAsync(string name)
	{
		var query = dataContext.GetData<Entities.Product>();

		if (!string.IsNullOrWhiteSpace(name))
		{
			query = query.Where(p => p.Name.Contains(name));
		}

		var dbProducts = await query.OrderBy(p => p.Name).Include(p => p.Category).ToListAsync();
		var products = new List<Product>();

		foreach (var dbProduct in dbProducts)
		{
			var product = mapper.Map<Product>(dbProduct);
			product.Category = mapper.Map<Category>(dbProduct.Category);
			products.Add(product);
		}

		return products;
	}

	public async Task<Product> SaveAsync(SaveProductRequest request)
	{
		var query = dataContext.GetData<Entities.Product>(trackingChanges: true);
		var dbProduct = request.Id != null ? await query.FirstOrDefaultAsync(p => p.Id == request.Id) : null;

		if (dbProduct == null)
		{
			var dbCategory = await dataContext.GetData<Entities.Category>().FirstOrDefaultAsync(c => c.Name == request.CategoryName);
			dbProduct = mapper.Map<Entities.Product>(request);
			dbProduct.CategoryId = dbCategory.Id;
			dataContext.Insert(dbProduct);
		}
		else
		{
			mapper.Map(request, dbProduct);
			dataContext.Edit(dbProduct);
		}

		await dataContext.SaveAsync();
		return mapper.Map<Product>(dbProduct);
	}
}