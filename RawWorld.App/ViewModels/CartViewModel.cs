namespace RawWorld.App.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;
using RawWorld.App.Models;
using RawWorld.App.Services;

public class CartViewModel : BaseViewModel
{
    private readonly ApiService _api;
    public ObservableCollection<CartItemModel> Items { get; } = [];
    public decimal Total => Items.Sum(i => (i.Product?.Price ?? 0) * i.Quantity);
    public ICommand LoadCommand { get; }
    public ICommand RemoveCommand { get; }
    public ICommand CheckoutCommand { get; }

    public CartViewModel(ApiService api)
    {
        _api = api;
        LoadCommand = new Command(async () => await Load());
        RemoveCommand = new Command<int>(async id => await Remove(id));
        CheckoutCommand = new Command(async () => await Checkout());
    }

    private async Task Load()
    {
        IsBusy = true;
        var items = await _api.GetCartAsync();
        Items.Clear();
        foreach (var i in items ?? []) Items.Add(i);
        OnPropertyChanged(nameof(Total));
        IsBusy = false;
    }

    private async Task Remove(int productId)
    {
        await _api.RemoveFromCartAsync(productId);
        await Load();
    }

    private async Task Checkout()
    {
        if (Items.Count == 0)
        {
            await Shell.Current.DisplayAlert("Cart", "Your cart is empty", "OK");
            return;
        }
        try
        {
            await Shell.Current.GoToAsync($"payment?total={Total}");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}