namespace RawWorld.API.Services;

using Microsoft.EntityFrameworkCore;
using RawWorld.API.Data;
using RawWorld.API.DTOs;
using RawWorld.API.Models;

public interface IOrderService
{
    Task<OrderDto?> PlaceOrderAsync(int userId, string address);
    Task<List<OrderDto>> GetUserOrdersAsync(int userId);
    Task<List<OrderDto>> GetAllOrdersAsync();
    Task<bool> UpdateStatusAsync(int orderId, string status);
}

public class OrderService(AppDbContext db, ICartService cart) : IOrderService
{
    public async Task<OrderDto?> PlaceOrderAsync(int userId, string address)
    {
        var items = await cart.GetCartAsync(userId);
        if (items.Count == 0) return null;

        var order = new Order
        {
            UserId = userId,
            Address = address,
            TotalAmount = items.Sum(i => i.Product!.Price * i.Quantity),
            Items = items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.Product!.Price
            }).ToList()
        };
        db.Orders.Add(order);
        await db.SaveChangesAsync();
        await cart.ClearAsync(userId);
        return ToDto(order);
    }

    public async Task<List<OrderDto>> GetUserOrdersAsync(int userId) =>
        await db.Orders
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .Where(o => o.UserId == userId)
            .Select(o => ToDto(o)).ToListAsync();

    public async Task<List<OrderDto>> GetAllOrdersAsync() =>
        await db.Orders
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .Select(o => ToDto(o)).ToListAsync();

    public async Task<bool> UpdateStatusAsync(int orderId, string status)
    {
        var o = await db.Orders.FindAsync(orderId);
        if (o == null) return false;
        o.Status = status; o.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return true;
    }

    private static OrderDto ToDto(Order o) => new(
        o.Id, o.TotalAmount, o.Status, o.PaymentStatus, o.CreatedAt,
        o.Items.Select(i => new OrderItemDto(
            i.Product?.Name ?? "", i.Quantity, i.UnitPrice)).ToList()
    );
}