using DependencyStore.Core.Orders.Create;
using Microsoft.AspNetCore.Mvc;

namespace DependencyStore.Web.Controllers;

public class OrderController : ControllerBase
{
    [Route("v1/orders")]
    [HttpPost]
    public async Task<IActionResult> Place(Request request)
    {
        var handler = new Handler();
        var result = await handler.HandleAsync(request);
        return Ok(result);
    }
}