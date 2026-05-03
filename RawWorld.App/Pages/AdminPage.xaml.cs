namespace RawWorld.App.Pages;

using RawWorld.App.ViewModels;

public partial class AdminPage : ContentPage
{
    private readonly AdminViewModel _vm;
    public AdminPage(AdminViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }
    protected override void OnAppearing() => _vm.LoadCommand.Execute(null);
}