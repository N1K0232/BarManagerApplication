using AutoMapper;
using BackendGestionaleBar.DataAccessLayer;

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
    }
}