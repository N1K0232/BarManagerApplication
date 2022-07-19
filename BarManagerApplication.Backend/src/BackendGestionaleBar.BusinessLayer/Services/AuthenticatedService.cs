using BackendGestionaleBar.Authentication.Entities;
using BackendGestionaleBar.BusinessLayer.Services.Interfaces;
using BackendGestionaleBar.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace BackendGestionaleBar.BusinessLayer.Services;

public sealed class AuthenticatedService : IAuthenticatedService
{
	private readonly UserManager<ApplicationUser> userManager;

	public AuthenticatedService(UserManager<ApplicationUser> userManager)
	{
		this.userManager = userManager;
	}

	public async Task<User> GetUserAsync(Guid userId)
	{
		if (userId == Guid.Empty)
		{
			return null;
		}

		var dbUser = await userManager.FindByIdAsync(userId.ToString());

		var user = new User
		{
			Name = dbUser.Name,
			DateOfBirth = dbUser.DateOfBirth,
			Email = dbUser.Email,
			UserName = dbUser.UserName,
			PhoneNumber = dbUser.PhoneNumber
		};

		return user;
	}
}