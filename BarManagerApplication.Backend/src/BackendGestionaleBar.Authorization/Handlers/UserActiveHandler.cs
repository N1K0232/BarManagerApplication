﻿using BackendGestionaleBar.Authentication.Entities;
using BackendGestionaleBar.Authentication.Extensions;
using BackendGestionaleBar.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BackendGestionaleBar.Authorization.Handlers;

public class UserActiveHandler : AuthorizationHandler<UserActiveRequirement>
{
    private readonly UserManager<AuthenticationUser> userManager;

    public UserActiveHandler(UserManager<AuthenticationUser> userManager)
    {
        this.userManager = userManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserActiveRequirement requirement)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var userId = context.User.GetId();
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user != null && user.LockoutEnd.GetValueOrDefault() <= DateTimeOffset.UtcNow)
            {
                context.Succeed(requirement);
            }
        }
    }
}