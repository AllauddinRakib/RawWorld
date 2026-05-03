namespace RawWorld.App.ViewModels;

using System.Windows.Input;
using RawWorld.App.Services;

[QueryProperty(nameof(TotalAmount), "total")]
public class PaymentViewModel : BaseViewModel
{
    private readonly ApiService _api;

    private bool _isCardSelected = true;
    private bool _isBkashSelected;
    private bool _isNagadSelected;

    public decimal TotalAmount { get; set; }
    public string CardNumber { get; set; } = "";
    public string CardExpiry { get; set; } = "";
    public string CardCvv { get; set; } = "";
    public string CardName { get; set; } = "";
    public string MobileNumber { get; set; } = "";
    public string Address { get; set; } = "";

    public bool IsCardSelected
    {
        get => _isCardSelected;
        set
        {
            _isCardSelected = value;
            _isBkashSelected = false;
            _isNagadSelected = false;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsBkashSelected));
            OnPropertyChanged(nameof(IsNagadSelected));
            OnPropertyChanged(nameof(PayButtonText));
        }
    }

    public bool IsBkashSelected
    {
        get => _isBkashSelected;
        set
        {
            _isBkashSelected = value;
            _isCardSelected = false;
            _isNagadSelected = false;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsCardSelected));
            OnPropertyChanged(nameof(IsNagadSelected));
            OnPropertyChanged(nameof(PayButtonText));
        }
    }

    public bool IsNagadSelected
    {
        get => _isNagadSelected;
        set
        {
            _isNagadSelected = value;
            _isCardSelected = false;
            _isBkashSelected = false;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsCardSelected));
            OnPropertyChanged(nameof(IsBkashSelected));
            OnPropertyChanged(nameof(PayButtonText));
        }
    }

    public string PayButtonText =>
        IsCardSelected ? "Pay with Card" :
        IsBkashSelected ? "Pay with bKash" :
                          "Pay with Nagad";

    public ICommand PayCommand { get; }

    public PaymentViewModel(ApiService api)
    {
        _api = api;
        PayCommand = new Command(async () => await ProcessPayment());
    }

    private async Task ProcessPayment()
    {
        if (string.IsNullOrWhiteSpace(Address))
        {
            await Shell.Current.DisplayAlert("Error", "Please enter a delivery address", "OK");
            return;
        }

        if (IsCardSelected)
        {
            if (CardNumber.Length < 16 || string.IsNullOrWhiteSpace(CardExpiry) ||
                string.IsNullOrWhiteSpace(CardCvv) || string.IsNullOrWhiteSpace(CardName))
            {
                await Shell.Current.DisplayAlert("Error", "Please fill in all card details", "OK");
                return;
            }
        }
        else
        {
            if (string.IsNullOrWhiteSpace(MobileNumber))
            {
                await Shell.Current.DisplayAlert("Error", "Please enter your mobile number", "OK");
                return;
            }
        }

        IsBusy = true;

        // Simulate payment processing delay
        await Task.Delay(2000);

        // Place the order
        var order = await _api.PlaceOrderAsync(Address);
        IsBusy = false;

        if (order == null)
        {
            await Shell.Current.DisplayAlert("Error", "Could not place order. Try again.", "OK");
            return;
        }

        string method = IsCardSelected ? "Card" : IsBkashSelected ? "bKash" : "Nagad";

        await Shell.Current.DisplayAlert("Payment Successful! 🎉",
            $"Order #{order.Id} confirmed!\n" +
            $"Payment via: {method}\n" +
            $"Amount: £{TotalAmount:F2}\n\n" +
            $"Your juice is on its way! 🥤", "OK");

        await Shell.Current.GoToAsync("//orders");
    }
}