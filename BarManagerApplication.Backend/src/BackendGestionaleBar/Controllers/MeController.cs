using BackendGestionaleBar.Authentication;
using BackendGestionaleBar.Authentication.Extensions;
using BackendGestionaleBar.Authentication.Filters;
using BackendGestionaleBar.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MeController : ControllerBase
    {
        [HttpGet("GetMe")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [RoleAuthorize(RoleNames.Administrator, RoleNames.Cliente)]
        public IActionResult GetMe()
        {
            var user = new User
            {
                FirstName = User.GetFirstName(),
                LastName = User.GetLastName(),
                BirthDate = User.GetBirthDate(),
                Email = User.GetEmail(),
                UserName = User.GetUserName(),
                TelephoneNumber = User.GetPhoneNumber()
            };

            return Ok(user);
        }
    }
}