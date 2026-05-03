namespace RawWorld.API.DTOs;

public record ProductDto(int Id, string Name, string Description,
                         decimal Price, int Stock, string? ImageUrl, string? Category);
public record CreateProductDto(string Name, string Description, decimal Price,
                               int StockQuantity, int? CategoryId, string? ImageUrl);