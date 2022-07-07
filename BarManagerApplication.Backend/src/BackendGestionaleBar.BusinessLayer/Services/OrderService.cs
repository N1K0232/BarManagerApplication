using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendGestionaleBar.BusinessLayer.Services.Interfaces;
using BackendGestionaleBar.Contracts;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.Shared.Enums;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Requests;
using Microsoft.EntityFrameworkCore;
using Entities = BackendGestionaleBar.DataAccessLayer.Entities;

namespace BackendGestionaleBar.BusinessLayer.Services;

public sealed class OrderService : IOrderService
{
	private readonly IBarManagerDataContext dataContext;
	private readonly IUserService userService;
	private readonly IMapper mapper;

	public OrderService(IBarManagerDataContext dataContext, IUserService userService, IMapper mapper)
	{
		this.dataContext = dataContext;
		this.userService = userService;
		this.mapper = mapper;
	}

	public async Task DeleteAsync(Guid? id)
	{
		if (id == null)
		{
			var dbOrders = await dataContext.GetData<Entities.Order>()
				.Where(o => o.OrderStatus == OrderStatus.Canceled)
				.ToListAsync();

			dataContext.Delete(dbOrders);
		}
		else
		{
			var dbOrder = await dataContext.GetAsync<Entities.Order>(id.Value);
			dataContext.Delete(dbOrder);
		}

		await dataContext.SaveAsync();
	}

	public async Task<IEnumerable<Order>> GetAsync()
	{
		var orders = await dataContext.GetData<Entities.Order>()
			.ProjectTo<Order>(mapper.ConfigurationProvider)
			.ToListAsync();

		return orders;
	}

	public async Task<decimal> GetTotalPriceAsync()
	{
		var dbOrder = await dataContext.GetData<Entities.Order>()
			.Include(o => o.OrderDetails)
			.FirstOrDefaultAsync(o => o.UserId == userService.GetId() && o.OrderDate == DateTime.Today);

		decimal totalPrice = 0;

		foreach (var orderDetail in dbOrder.OrderDetails)
		{
			totalPrice += orderDetail.Price * orderDetail.OrderedQuantity;
		}

		return totalPrice;
	}

	public async Task<Order> SaveAsync(SaveOrderRequest request)
	{
		var query = dataContext.GetData<Entities.Order>(trackingChanges: true);
		var dbOrder = request.Id != null ? await query.FirstOrDefaultAsync(o => o.Id == request.Id) : null;

		if (dbOrder == null)
		{
			dbOrder = new Entities.Order
			{
				UserId = userService.GetId(),
				OrderDate = DateTime.UtcNow,
				OrderStatus = OrderStatus.New,
				OrderDetails = new List<Entities.OrderDetail>()
			};

			foreach (var product in request.Products)
			{
				var dbProduct = await dataContext.GetAsync<Entities.Product>(product.Id);

				if (dbProduct.Quantity < request.OrderedQuantity)
				{
					throw new Exception($"you can order a maximum of {dbProduct.Quantity}");
				}

				var orderDetail = new Entities.OrderDetail
				{
					OrderId = dbOrder.Id,
					ProductId = dbProduct.Id,
					Price = dbProduct.Price,
					OrderedQuantity = request.OrderedQuantity
				};

				dbOrder.OrderDetails.Add(orderDetail);

				dbProduct.Quantity -= request.OrderedQuantity;
				dataContext.Edit(dbProduct);
			}

			dataContext.Insert(dbOrder);
		}
		else
		{
			mapper.Map(request, dbOrder);
			dataContext.Edit(dbOrder);
		}

		await dataContext.SaveAsync();
		return mapper.Map<Order>(dbOrder);
	}
}