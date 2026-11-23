using BloodPressure.Maui.ViewModels;

namespace BloodPressure.Maui.Views;

public partial class CreateUserView : ContentPage
{
	public CreateUserView(UserViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}