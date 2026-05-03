namespace RawWorld.App.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;
using RawWorld.App.Models;
using RawWorld.App.Services;

public class OrderListViewModel : BaseViewModel
{
    private readonly ApiService _api;
    public ObservableCollection<OrderModel> Orders { get; } = [];
    public ICommand LoadCommand { get; }
    public ICommand LogoutCommand { get; }

    public OrderListViewModel(ApiService api)
    {
        _api = api;
        LoadCommand = new Command(async () => await Load());
        LogoutCommand = new Command(async () => await Logout());
    }

    private async Task Load()
    {
        IsBusy = true;
        var list = await _api.GetMyOrdersAsync();
        Orders.Clear();
        foreach (var o in list ?? []) Orders.Add(o);
        IsBusy = false;
    }

    private async Task Logout()
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Logout", "Are you sure you want to logout?", "Yes", "No");
        if (!confirm) return;

        SessionService.Clear();
        _api.SetToken("");
        await Shell.Current.GoToAsync("//login");
    }
}