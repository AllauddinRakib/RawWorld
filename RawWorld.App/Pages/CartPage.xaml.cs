namespace RawWorld.App.Pages;

using RawWorld.App.ViewModels;

public partial class CartPage : ContentPage
{
    private readonly CartViewModel _vm;
    public CartPage(CartViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }
    protected override void OnAppearing() => _vm.LoadCommand.Execute(null);
}