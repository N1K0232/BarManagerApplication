using AutoMapper;
using BackendGestionaleBar.DataAccessLayer;
using BackendGestionaleBar.Shared.Models;
using BackendGestionaleBar.Shared.Models.Enums;
using BackendGestionaleBar.Shared.Models.Requests;
using Microsoft.EntityFrameworkCore;
using ApplicationOrder = BackendGestionaleBar.DataAccessLayer.Entities.Order;
using ApplicationOrderDetail = BackendGestionaleBar.DataAccessLayer.Entities.OrderDetail;

namespace BackendGestionaleBar.BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDataContext dataContext;
        private readonly IDatabase database;
        private readonly IMapper mapper;

        public OrderService(IDataContext dataContext, IDatabase database, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.database = database;
            this.mapper = mapper;
        }

        public async Task DeleteAsync()
        {
            var query = dataContext.GetData<ApplicationOrder>(trackingChanges: true);
            query = query.Where(o => o.OrderStatus == OrderStatus.Canceled || o.OrderStatus == OrderStatus.Completed);
            var orders = await query.ToListAsync();
            dataContext.Delete(orders);
            await dataContext.SaveAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var order = await dataContext.GetAsync<ApplicationOrder>(id);
            if (order != null)
            {
                dataContext.Delete(order);
                await dataContext.SaveAsync();
            }
        }
        public async Task<Order> SaveAsync(SaveOrderRequest request)
        {
            var dbOrder = request.Id != null ?
                await dataContext.GetAsync<ApplicationOrder>(request.Id.Value)
                : null;

            if (dbOrder == null)
            {
                dbOrder = mapper.Map<ApplicationOrder>(request);
                dbOrder.OrderStatus = OrderStatus.New;
                dbOrder.OrderDate = DateTime.UtcNow;
                dataContext.Insert(dbOrder);

                var orderDetail = new ApplicationOrderDetail
                {
                    IdOrder = request.Id.Value,
                    IdProduct = request.IdProduct
                };

                dataContext.Insert(orderDetail);
            }
            else
            {
                dbOrder.LastModifiedDate = DateTime.UtcNow;
            }

            await dataContext.SaveAsync();
            return mapper.Map<Order>(dbOrder);
        }
        public async Task<Order> GetOrderAsync(Guid id)
        {
            var dbOrder = await dataContext.GetAsync<ApplicationOrder>(id);
            var order = mapper.Map<Order>(dbOrder);
            var totalPrice = await database.GetPriceAsync(id);
            order.TotalPrice = totalPrice;
            return order;
        }
    }
}