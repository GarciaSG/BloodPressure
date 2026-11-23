using BloodPressure.Maui.ViewModels;

namespace BloodPressure.Maui.Views;

public partial class BloodPressureFormPage : ContentPage
{
    public BloodPressureFormPage(BloodPressureViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}