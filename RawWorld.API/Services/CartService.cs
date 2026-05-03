namespace RawWorld.API.Services;

using Microsoft.EntityFrameworkCore;
using RawWorld.API.Data;
using RawWorld.API.Models;

public interface ICartService
{
    Task<List<CartItem>> GetCartAsync(int userId);
    Task AddOrUpdateAsync(int userId, int productId, int qty);
    Task RemoveAsync(int userId, int productId);
    Task ClearAsync(int userId);
}

public class CartService(AppDbContext db) : ICartService
{
    public Task<List<CartItem>> GetCartAsync(int userId) =>
        db.CartItems.Include(c => c.Product).Where(c => c.UserId == userId).ToListAsync();

    public async Task AddOrUpdateAsync(int userId, int productId, int qty)
    {
        var item = await db.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
        if (item == null)
            db.CartItems.Add(new CartItem { UserId = userId, ProductId = productId, Quantity = qty });
        else
            item.Quantity = qty;
        await db.SaveChangesAsync();
    }

    public async Task RemoveAsync(int userId, int productId)
    {
        var item = await db.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
        if (item != null) { db.CartItems.Remove(item); await db.SaveChangesAsync(); }
    }

    public async Task ClearAsync(int userId)
    {
        db.CartItems.RemoveRange(db.CartItems.Where(c => c.UserId == userId));
        await db.SaveChangesAsync();
    }
}