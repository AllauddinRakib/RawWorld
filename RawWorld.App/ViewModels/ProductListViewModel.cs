namespace RawWorld.App.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;
using RawWorld.App.Models;
using RawWorld.App.Services;

public class ProductListViewModel : BaseViewModel
{
    private readonly ApiService _api;
    public ObservableCollection<ProductModel> Products { get; } = [];
    public ICommand LoadCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand AddToCartCommand { get; }
   
    public string SearchText { get; set; } = "";

    public ProductListViewModel(ApiService api)
    {
        _api = api;
        LoadCommand = new Command(async () => await Load());
        SearchCommand = new Command(async () => await Load(SearchText));
        AddToCartCommand = new Command<int>(async id => await AddToCart(id));
       
    }

    private async Task Load(string? search = null)
    {
        IsBusy = true;
        try
        {
            var list = await _api.GetProductsAsync(search);
            Products.Clear();
            foreach (var p in list ?? []) Products.Add(p);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        IsBusy = false;
    }

    private async Task AddToCart(int productId)
    {
        await _api.AddToCartAsync(productId);
        await Shell.Current.DisplayAlert("Cart", "Item added to cart!", "OK");
    }
}