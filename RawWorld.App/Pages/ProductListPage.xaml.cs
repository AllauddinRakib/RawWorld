namespace RawWorld.App.Pages;

using RawWorld.App.ViewModels;

public partial class ProductListPage : ContentPage
{
    private readonly ProductListViewModel _vm;

    public ProductListPage(ProductListViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override void OnAppearing() => _vm.LoadCommand.Execute(null);

    private void OnAddToCartClicked(object? sender, EventArgs e)
    {
        if (sender is not Button btn) return;
        if (btn.CommandParameter is not int productId) return;
        _vm.AddToCartCommand.Execute(productId);
    }
}