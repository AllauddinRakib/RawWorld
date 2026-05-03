namespace RawWorld.App.Pages;

using RawWorld.App.ViewModels;

public partial class AdminProductPage : ContentPage
{
    private readonly AdminProductViewModel _vm;
    public AdminProductPage(AdminProductViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }
    protected override void OnAppearing() => _vm.LoadCommand.Execute(null);
}