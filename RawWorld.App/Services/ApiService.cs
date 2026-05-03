namespace RawWorld.App.Services;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using RawWorld.App.Models;

public class ApiService
{
    private readonly HttpClient _http;
    private const string Base = "https://10.0.2.2:7055";

    public ApiService()
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => true;
        _http = new HttpClient(handler) { BaseAddress = new Uri(Base) };
    }

    public void SetToken(string token) =>
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

    public Task<AuthResponse?> RegisterAsync(RegisterRequest r) =>
        PostJson<AuthResponse>("/api/auth/register", r);

    public Task<AuthResponse?> LoginAsync(string email, string pass) =>
        PostJson<AuthResponse>("/api/auth/login", new { email, password = pass });

    public Task<AuthResponse?> RefreshAsync(string rt) =>
        PostJson<AuthResponse>("/api/auth/refresh", new { refreshToken = rt });

    public Task<List<ProductModel>?> GetProductsAsync(string? search = null)
    {
        var url = "/api/products" + (search != null ? $"?search={Uri.EscapeDataString(search)}" : "");
        return _http.GetFromJsonAsync<List<ProductModel>>(url);
    }

    public Task<ProductModel?> GetProductAsync(int id) =>
        _http.GetFromJsonAsync<ProductModel>($"/api/products/{id}");

    public Task<List<CartItemModel>?> GetCartAsync() =>
        _http.GetFromJsonAsync<List<CartItemModel>>("/api/cart");

    public Task AddToCartAsync(int productId, int qty = 1) =>
        _http.PostAsync($"/api/cart/{productId}?qty={qty}", null);

    public Task RemoveFromCartAsync(int productId) =>
        _http.DeleteAsync($"/api/cart/{productId}");

    public Task<OrderModel?> PlaceOrderAsync(string address) =>
        PostJson<OrderModel>("/api/orders", new { address });

    public Task<List<OrderModel>?> GetMyOrdersAsync() =>
        _http.GetFromJsonAsync<List<OrderModel>>("/api/orders");

    public Task<List<OrderModel>?> GetAllOrdersAsync() =>
        _http.GetFromJsonAsync<List<OrderModel>>("/api/admin/orders");

    public Task UpdateOrderStatusAsync(int id, string status) =>
        _http.PutAsync($"/api/admin/orders/{id}/status?status={status}", null);

    private async Task<T?> PostJson<T>(string url, object body)
    {
        var res = await _http.PostAsJsonAsync(url, body);
        return res.IsSuccessStatusCode
            ? await res.Content.ReadFromJsonAsync<T>()
            : default;
    }
    public async Task<ProductModel?> CreateProductAsync(string name, string description,
    decimal price, int stock, int categoryId)
    {
        var body = new
        {
            name,
            description,
            price,
            stockQuantity = stock,
            categoryId,
            imageUrl = ""
        };
        return await PostJson<ProductModel>("/api/products", body);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var res = await _http.DeleteAsync($"/api/products/{id}");
        return res.IsSuccessStatusCode;
    }
}