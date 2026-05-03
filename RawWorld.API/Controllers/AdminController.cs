namespace RawWorld.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawWorld.API.Services;

[ApiController, Route("api/admin"), Authorize(Roles = "Admin")]
public class AdminController(IOrderService orderSvc) : ControllerBase
{
    [HttpGet("orders")]
    public async Task<IActionResult> AllOrders() =>
        Ok(await orderSvc.GetAllOrdersAsync());

    [HttpPut("orders/{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status) =>
        await orderSvc.UpdateStatusAsync(id, status) ? Ok() : NotFound();
}