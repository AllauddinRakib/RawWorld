namespace RawWorld.API.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawWorld.API.Services;

[ApiController, Route("api/cart"), Authorize]
public class CartController(ICartService svc) : ControllerBase
{
    private int UserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await svc.GetCartAsync(UserId));

    [HttpPost("{productId}")]
    public async Task<IActionResult> AddOrUpdate(int productId, [FromQuery] int qty = 1)
    {
        await svc.AddOrUpdateAsync(UserId, productId, qty);
        return Ok();
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> Remove(int productId)
    {
        await svc.RemoveAsync(UserId, productId);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Clear()
    {
        await svc.ClearAsync(UserId);
        return Ok();
    }
}