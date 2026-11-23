using BloodPressure.Maui.ViewModels;

namespace BloodPressure.Maui.Views;

public partial class LoginView : ContentPage
{
	public LoginView(UserViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}