namespace RawWorld.App.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;
using RawWorld.App.Models;
using RawWorld.App.Services;

public class AdminViewModel : BaseViewModel
{
    private readonly ApiService _api;
    public ObservableCollection<OrderModel> Orders { get; } = [];
    public ICommand LoadCommand { get; }
    public ICommand UpdateStatusCommand { get; }

    public AdminViewModel(ApiService api)
    {
        _api = api;
        LoadCommand = new Command(async () => await Load());
        UpdateStatusCommand = new Command<OrderModel>(async o => await UpdateStatus(o));
    }

    private async Task Load()
    {
        IsBusy = true;
        var list = await _api.GetAllOrdersAsync();
        Orders.Clear();
        foreach (var o in list ?? []) Orders.Add(o);
        IsBusy = false;
    }

    private async Task UpdateStatus(OrderModel order)
    {
        var statuses = new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
        var choice = await Shell.Current.DisplayActionSheet(
            $"Update Order #{order.Id}", "Cancel", null, statuses);
        if (choice == null || choice == "Cancel") return;
        await _api.UpdateOrderStatusAsync(order.Id, choice);
        await Load();
    }
}