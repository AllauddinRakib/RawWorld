namespace RawWorld.API.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RawWorld.API.DTOs;
using RawWorld.API.Services;

[ApiController, Route("api/orders"), Authorize]
public class OrderController(IOrderService svc) : ControllerBase
{
    private int UserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(PlaceOrderDto dto)
    {
        var order = await svc.PlaceOrderAsync(UserId, dto.Address);
        return order == null ? BadRequest("Cart is empty") : Ok(order);
    }

    [HttpGet]
    public async Task<IActionResult> MyOrders() =>
        Ok(await svc.GetUserOrdersAsync(UserId));
}