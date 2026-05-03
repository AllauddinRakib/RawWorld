namespace RawWorld.API.Services;

using Microsoft.EntityFrameworkCore;
using RawWorld.API.Data;
using RawWorld.API.DTOs;
using RawWorld.API.Models;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync(string? search);
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<bool> UpdateAsync(int id, CreateProductDto dto);
    Task<bool> DeleteAsync(int id);
}

public class ProductService(AppDbContext db) : IProductService
{
    public async Task<List<ProductDto>> GetAllAsync(string? search)
    {
        var q = db.Products.Include(p => p.Category).Where(p => p.IsActive);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
        return await q.Select(p => ToDto(p)).ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var p = await db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        return p == null ? null : ToDto(p);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var p = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId,
            ImageUrl = dto.ImageUrl
        };
        db.Products.Add(p);
        await db.SaveChangesAsync();
        return ToDto(p);
    }

    public async Task<bool> UpdateAsync(int id, CreateProductDto dto)
    {
        var p = await db.Products.FindAsync(id);
        if (p == null) return false;
        p.Name = dto.Name; p.Description = dto.Description;
        p.Price = dto.Price; p.StockQuantity = dto.StockQuantity;
        p.CategoryId = dto.CategoryId; p.ImageUrl = dto.ImageUrl;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var p = await db.Products.FindAsync(id);
        if (p == null) return false;
        p.IsActive = false;
        await db.SaveChangesAsync();
        return true;
    }

    private static ProductDto ToDto(Product p) =>
        new(p.Id, p.Name, p.Description, p.Price,
            p.StockQuantity, p.ImageUrl, p.Category?.Name);
}