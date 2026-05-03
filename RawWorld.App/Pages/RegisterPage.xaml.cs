namespace RawWorld.App.Pages;

using RawWorld.App.ViewModels;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}