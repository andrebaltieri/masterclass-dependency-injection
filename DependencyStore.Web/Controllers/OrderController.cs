using DependencyStore.Core.Orders.Create;
using Microsoft.AspNetCore.Mvc;

namespace DependencyStore.Web.Controllers;

public class OrderController : ControllerBase
{
    [Route("v1/orders")]
    [HttpPost]
    public async Task<IActionResult> Place(
        [FromBody] Request request,
        [FromServices] Handler handler)
    {
        var result = await handler.HandleAsync(request);
        return Ok(result);
    }
}