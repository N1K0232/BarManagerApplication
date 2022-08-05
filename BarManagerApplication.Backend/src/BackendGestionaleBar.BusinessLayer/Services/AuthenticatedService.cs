using AutoMapper;
using BackendGestionaleBar.Authentication.Entities;
using BackendGestionaleBar.BusinessLayer.Services.Interfaces;
using BackendGestionaleBar.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace BackendGestionaleBar.BusinessLayer.Services;

public sealed class AuthenticatedService : IAuthenticatedService
{
	private readonly UserManager<AuthenticationUser> userManager;
	private readonly IMapper mapper;

	public AuthenticatedService(UserManager<AuthenticationUser> userManager, IMapper mapper)
	{
		this.userManager = userManager;
		this.mapper = mapper;
	}

	public async Task<User> GetUserAsync(Guid userId)
	{
		if (userId == Guid.Empty)
		{
			return null;
		}

		string id = Convert.ToString(userId);

		var dbUser = await userManager.FindByIdAsync(id);

		var user = mapper.Map<User>(dbUser);
		return user;
	}
}