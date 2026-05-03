namespace RawWorld.App.Pages;

using RawWorld.App.ViewModels;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}