﻿using Microsoft.AspNetCore.Authorization;

namespace BackendGestionaleBar.Authorization;

public class RoleAuthorizeAttribute : AuthorizeAttribute
{
    public RoleAuthorizeAttribute(params string[] roles)
    {
        Roles = string.Join(",", roles);
    }
}