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
    private readonly IDataContext dataContext;
    private readonly IUserService userService;
    private readonly IAuthenticatedService authenticatedService;
    private readonly IMapper mapper;

    public OrderService(IDataContext dataContext, IUserService userService, IAuthenticatedService authenticatedService, IMapper mapper)
    {
        this.dataContext = dataContext;
        this.userService = userService;
        this.authenticatedService = authenticatedService;
        this.mapper = mapper;
    }

    public async Task DeleteAsync(Guid id)
    {
        var order = await dataContext.GetAsync<Entities.Order>(id);
        var orderDetails = await dataContext.OrderDetails.Where(o => o.OrderId == id).ToListAsync();
        dataContext.Delete(order);
        dataContext.OrderDetails.RemoveRange(orderDetails);
        await dataContext.SaveAsync();
    }

    public async Task<Order> GetYourOrderAsync()
    {
        var dbOrder = await dataContext.GetData<Entities.Order>()
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.UserId == userService.GetId() && o.OrderDate == DateTime.Today);

        var products = new List<Product>();

        foreach (var orderDetail in dbOrder.OrderDetails)
        {
            var product = mapper.Map<Product>(orderDetail.Product);
            products.Add(product);
        }

        var order = mapper.Map<Order>(dbOrder);
        order.Products = products;
        return order;
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
        Guid? userId = userService.GetId();

        var query = dataContext.GetData<Entities.Order>(trackingChanges: true);
        var dbOrder = request.Id != null ? await query.FirstOrDefaultAsync(o => o.Id == request.Id) : null;

        if (dbOrder == null)
        {
            var dbUmbrella = await dataContext.GetData<Entities.Umbrella>().FirstOrDefaultAsync(u => u.Coordinates == request.Umbrella);

            dbOrder = new Entities.Order
            {
                UserId = userId.GetValueOrDefault(Guid.Empty),
                UmbrellaId = dbUmbrella.Id,
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

        var savedOrder = mapper.Map<Order>(dbOrder);
        savedOrder.User = await authenticatedService.GetUserAsync(userId.Value);
        return savedOrder;
    }
}