namespace RawWorld.App.Services;

public static class SessionService
{
    public static string? AccessToken { get; private set; }
    public static string? RefreshToken { get; private set; }
    public static string? Role { get; private set; }
    public static bool IsLoggedIn => AccessToken != null;
    public static bool IsAdmin => Role == "Admin";

    public static void SetSession(string access, string refresh, string role)
    {
        AccessToken = access;
        RefreshToken = refresh;
        Role = role;
        SecureStorage.Default.SetAsync("access_token", access);
        SecureStorage.Default.SetAsync("refresh_token", refresh);
        SecureStorage.Default.SetAsync("role", role);
    }

    public static async Task LoadFromStorageAsync()
    {
        AccessToken = await SecureStorage.Default.GetAsync("access_token");
        RefreshToken = await SecureStorage.Default.GetAsync("refresh_token");
        Role = await SecureStorage.Default.GetAsync("role");
    }

    public static void Clear()
    {
        AccessToken = RefreshToken = Role = null;
        SecureStorage.Default.Remove("access_token");
        SecureStorage.Default.Remove("refresh_token");
        SecureStorage.Default.Remove("role");
    }
}