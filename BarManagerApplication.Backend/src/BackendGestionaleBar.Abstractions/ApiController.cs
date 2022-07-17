using Microsoft.AspNetCore.Mvc;

namespace BackendGestionaleBar.Abstractions;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase
{
	protected ApiController() : base()
	{
	}
}