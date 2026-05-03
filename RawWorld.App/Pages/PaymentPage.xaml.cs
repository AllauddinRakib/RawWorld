namespace RawWorld.App.Pages;

using RawWorld.App.ViewModels;

public partial class PaymentPage : ContentPage
{
    public PaymentPage(PaymentViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}