using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authentication.Extensions;
using BackendGestionaleBar.Authentication.Filters;
using BackendGestionaleBar.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MeController : ControllerBase
    {
        [HttpGet("GetMe")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [RoleAuthorize(RoleNames.Administrator, RoleNames.Staff, RoleNames.Cliente)]
        public IActionResult GetMe()
        {
            var user = new User
            {
                Id = User.GetId(),
                FirstName = User.GetFirstName(),
                LastName = User.GetLastName(),
                BirthDate = User.GetBirthDate(),
                Email = User.GetEmail(),
                UserName = User.GetUserName(),
                PhoneNumber = User.GetPhoneNumber()
            };

            return Ok(user);
        }
    }
}