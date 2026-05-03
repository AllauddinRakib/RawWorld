namespace RawWorld.App.Models;

public record AuthResponse(string AccessToken, string RefreshToken, string Role);
public record RegisterRequest(string FullName, string Email, string Password);

public class ProductModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? ImageUrl { get; set; }
    public string? Category { get; set; }
}

public class CartItemModel
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public ProductModel? Product { get; set; }
}

public class OrderModel
{
    public int Id { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = "";
    public string PaymentStatus { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public List<OrderItemModel> Items { get; set; } = [];
}

public class OrderItemModel
{
    public string ProductName { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}