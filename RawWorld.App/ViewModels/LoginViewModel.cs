namespace RawWorld.App.ViewModels;

using System.Windows.Input;
using RawWorld.App.Services;

public class LoginViewModel : BaseViewModel
{
    private readonly ApiService _api;
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";

    public ICommand CustomerLoginCommand { get; }
    public ICommand AdminLoginCommand { get; }
    public ICommand RegisterCommand { get; }

    public LoginViewModel(ApiService api)
    {
        _api = api;
        CustomerLoginCommand = new Command(async () => await DoLogin("Customer"));
        AdminLoginCommand = new Command(async () => await DoLogin("Admin"));
        RegisterCommand = new Command(async () =>
            await Shell.Current.GoToAsync("//register"));
    }

    private async Task DoLogin(string expectedRole)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Error",
                    "Please fill in all fields", "OK");
                return;
            }

            IsBusy = true;
            var res = await _api.LoginAsync(Email, Password);
            IsBusy = false;

            if (res == null)
            {
                await Shell.Current.DisplayAlert("Error",
                    "Invalid email or password", "OK");
                return;
            }

            if (expectedRole == "Admin" && res.Role != "Admin")
            {
                await Shell.Current.DisplayAlert("Access Denied",
                    "This account does not have admin access. Please use Customer login.", "OK");
                return;
            }

            if (expectedRole == "Customer" && res.Role == "Admin")
            {
                await Shell.Current.DisplayAlert("Access Denied",
                    "Admin accounts must use the Admin login button.", "OK");
                return;
            }

            SessionService.SetSession(res.AccessToken, res.RefreshToken, res.Role);
            _api.SetToken(res.AccessToken);

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Shell.Current is AppShell shell)
                    shell.SetUserRole(res.Role);
            });

            await Shell.Current.GoToAsync("//products");
        }
        catch (Exception ex)
        {
            IsBusy = false;
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}