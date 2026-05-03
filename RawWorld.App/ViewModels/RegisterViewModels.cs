namespace RawWorld.App.ViewModels;

using System.Windows.Input;
using RawWorld.App.Models;
using RawWorld.App.Services;

public class RegisterViewModel : BaseViewModel
{
    private readonly ApiService _api;
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public ICommand RegisterCommand { get; }
    public ICommand BackToLoginCommand { get; }

    public RegisterViewModel(ApiService api)
    {
        _api = api;
        RegisterCommand = new Command(async () => await DoRegister());
        BackToLoginCommand = new Command(async () =>
            await Shell.Current.GoToAsync("//login"));
    }

    private async Task DoRegister()
    {
        if (string.IsNullOrWhiteSpace(FullName) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlert("Error", "Please fill in all fields", "OK");
            return;
        }

        if (Password.Length < 6)
        {
            await Shell.Current.DisplayAlert("Error", "Password must be at least 6 characters", "OK");
            return;
        }

        IsBusy = true;
        var res = await _api.RegisterAsync(new RegisterRequest(FullName, Email, Password));
        IsBusy = false;

        if (res == null)
        {
            await Shell.Current.DisplayAlert("Error", "Registration failed. Email may already be in use.", "OK");
            return;
        }

        SessionService.SetSession(res.AccessToken, res.RefreshToken, res.Role);
        if (res.Role == "Admin")
            await Shell.Current.GoToAsync("//adminproducts");
        else
            await Shell.Current.GoToAsync("//products");
    }
}