namespace RawWorld.App;

using RawWorld.App.Pages;
using RawWorld.App.Services;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("payment", typeof(PaymentPage));
    }

    public void SetUserRole(string role)
    {
        bool isAdmin = role == "Admin";
        AdminTab.IsVisible = isAdmin;
        ManageTab.IsVisible = isAdmin;
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(
            "Logout", "Are you sure you want to logout?", "Yes", "No");
        if (!confirm) return;
        SessionService.Clear();
        AdminTab.IsVisible = false;
        ManageTab.IsVisible = false;
        await GoToAsync("//login");
    }
}