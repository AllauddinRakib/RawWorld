namespace RawWorld.App.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;
using RawWorld.App.Models;
using RawWorld.App.Services;

public class AdminProductViewModel : BaseViewModel
{
    private readonly ApiService _api;

    public ObservableCollection<ProductModel> Products { get; } = [];
    public ObservableCollection<string> Categories { get; } =
        ["Juices", "Smoothies", "Detox Drinks"];

    public string NewName { get; set; } = "";
    public string NewDescription { get; set; } = "";
    public string NewPrice { get; set; } = "";
    public string NewStock { get; set; } = "";
    public string? SelectedCategory { get; set; } = "Juices";

    public ICommand LoadCommand { get; }
    public ICommand AddProductCommand { get; }
    public ICommand DeleteProductCommand { get; }

    public AdminProductViewModel(ApiService api)
    {
        _api = api;
        LoadCommand = new Command(async () => await Load());
        AddProductCommand = new Command(async () => await AddProduct());
        DeleteProductCommand = new Command<int>(async id => await DeleteProduct(id));
    }

    private async Task Load()
    {
        IsBusy = true;
        var list = await _api.GetProductsAsync();
        Products.Clear();
        foreach (var p in list ?? []) Products.Add(p);
        IsBusy = false;
    }

    private async Task AddProduct()
    {
        if (string.IsNullOrWhiteSpace(NewName) ||
            string.IsNullOrWhiteSpace(NewDescription) ||
            string.IsNullOrWhiteSpace(NewPrice) ||
            string.IsNullOrWhiteSpace(NewStock))
        {
            await Shell.Current.DisplayAlert("Error",
                "Please fill in all fields", "OK");
            return;
        }

        if (!decimal.TryParse(NewPrice, out decimal price))
        {
            await Shell.Current.DisplayAlert("Error", "Invalid price", "OK");
            return;
        }

        if (!int.TryParse(NewStock, out int stock))
        {
            await Shell.Current.DisplayAlert("Error",
                "Invalid stock quantity", "OK");
            return;
        }

        int categoryId = SelectedCategory switch
        {
            "Smoothies" => 2,
            "Detox Drinks" => 3,
            _ => 1
        };

        IsBusy = true;
        var result = await _api.CreateProductAsync(
            NewName, NewDescription, price, stock, categoryId);
        IsBusy = false;

        if (result != null)
        {
            await Shell.Current.DisplayAlert("Success",
                $"'{NewName}' added successfully!", "OK");

            // Clear form
            NewName = "";
            NewDescription = "";
            NewPrice = "";
            NewStock = "";
            SelectedCategory = "Juices";
            OnPropertyChanged(nameof(NewName));
            OnPropertyChanged(nameof(NewDescription));
            OnPropertyChanged(nameof(NewPrice));
            OnPropertyChanged(nameof(NewStock));
            OnPropertyChanged(nameof(SelectedCategory));

            await Load();
        }
        else
        {
            await Shell.Current.DisplayAlert("Error",
                "Could not add product. Try again.", "OK");
        }
    }

    private async Task DeleteProduct(int productId)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Delete Product",
            "Are you sure you want to delete this product?",
            "Delete", "Cancel");
        if (!confirm) return;

        IsBusy = true;
        var success = await _api.DeleteProductAsync(productId);
        IsBusy = false;

        if (success)
        {
            await Shell.Current.DisplayAlert("Success",
                "Product deleted successfully", "OK");
            await Load();
        }
        else
        {
            await Shell.Current.DisplayAlert("Error",
                "Could not delete product", "OK");
        }
    }
}