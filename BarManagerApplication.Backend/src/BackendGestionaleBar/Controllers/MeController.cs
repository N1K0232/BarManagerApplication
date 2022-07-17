using BackendGestionaleBar.Abstractions.Controllers;
using BackendGestionaleBar.Abstractions.Filters;
using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authentication.Extensions;
using BackendGestionaleBar.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers;

public class MeController : ApiController
{
    [HttpGet("GetMe")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [RoleAuthorize(RoleNames.Administrator, RoleNames.Staff, RoleNames.Customer)]
    public IActionResult GetMe()
    {
        var user = new User
        {
            Id = User.GetId(),
            Name = User.GetName(),
            DateOfBirth = User.GetDateOfBirth(),
            Email = User.GetEmail(),
            UserName = User.GetUserName(),
            PhoneNumber = User.GetPhoneNumber()
        };

        return Ok(user);
    }
}