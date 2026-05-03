namespace RawWorld.App.Pages;

using RawWorld.App.ViewModels;

public partial class OrderListPage : ContentPage
{
    private readonly OrderListViewModel _vm;
    public OrderListPage(OrderListViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }
    protected override void OnAppearing() => _vm.LoadCommand.Execute(null);
}