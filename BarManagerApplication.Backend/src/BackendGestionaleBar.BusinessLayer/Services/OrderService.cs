using AutoMapper;
using BackendGestionaleBar.BusinessLayer.Services.Common;
using BackendGestionaleBar.Contracts;
using BackendGestionaleBar.DataAccessLayer;

namespace BackendGestionaleBar.BusinessLayer.Services;

public class OrderService : IOrderService
{
	private readonly IDataContext dataContext;
	private readonly IUserService userService;
	private readonly IMapper mapper;

	public OrderService(IDataContext dataContext, IUserService userService, IMapper mapper)
	{
		this.dataContext = dataContext;
		this.userService = userService;
		this.mapper = mapper;
	}
}