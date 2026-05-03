namespace RawWorld.API.DTOs;

public record PlaceOrderDto(string Address);
public record OrderDto(int Id, decimal Total, string Status,
                       string PaymentStatus, DateTime CreatedAt,
                       List<OrderItemDto> Items);
public record OrderItemDto(string ProductName, int Quantity, decimal UnitPrice);