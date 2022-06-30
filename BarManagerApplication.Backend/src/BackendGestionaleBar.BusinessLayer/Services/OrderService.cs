using AutoMapper;
using AutoMapper.QueryableExtensions;
using BackendGestionaleBar.BusinessLayer.Services.Common;
using BackendGestionaleBar.Contracts;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.Shared.Enums;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Requests;
using Microsoft.EntityFrameworkCore;
using Entities = BackendGestionaleBar.DataAccessLayer.Entities;

namespace BackendGestionaleBar.BusinessLayer.Services;

public class OrderService : IOrderService
{
	private readonly IApplicationDataContext dataContext;
	private readonly IUserService userService;
	private readonly IMapper mapper;

	public OrderService(IApplicationDataContext dataContext, IUserService userService, IMapper mapper)
	{
		this.dataContext = dataContext;
		this.userService = userService;
		this.mapper = mapper;
	}

	public async Task DeleteAsync(Guid id)
	{
		var order = await dataContext.GetAsync<Entities.Order>(id);
		dataContext.Delete(order);
		await dataContext.SaveAsync();
	}

	public async Task<IEnumerable<Order>> GetAsync()
	{
		var orders = await dataContext.GetData<Entities.Order>()
			.ProjectTo<Order>(mapper.ConfigurationProvider)
			.ToListAsync();

		return orders;
	}

	public async Task<decimal> GetTotalPriceAsync(DateTime orderDate)
	{
		var dbOrder = await dataContext.GetData<Entities.Order>()
			.Include(o => o.OrderDetails)
			.FirstOrDefaultAsync(o => o.UserId == userService.GetId() && o.OrderDate == orderDate);

		decimal totalPrice = 0;

		foreach (var orderDetail in dbOrder.OrderDetails)
		{
			totalPrice += orderDetail.Price;
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
				OrderStatus = OrderStatus.New
			};

			dbOrder.OrderDetails = new List<Entities.OrderDetail>();

			foreach (var product in request.Products)
			{
				dbOrder.OrderDetails.Add(new Entities.OrderDetail
				{
					OrderId = dbOrder.Id,
					ProductId = product.Id,
					Price = product.Price,
					OrderedQuantity = request.OrderedQuantity
				});
			}

			dataContext.Insert(dbOrder);
		}
		else
		{
			dbOrder.OrderStatus = request.Status.Value;
			dataContext.Edit(dbOrder);
		}

		await dataContext.SaveAsync();
		return mapper.Map<Order>(dbOrder);
	}
}