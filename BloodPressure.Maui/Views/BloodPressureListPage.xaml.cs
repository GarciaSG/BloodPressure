using BloodPressure.Maui.ViewModels;

namespace BloodPressure.Maui.Views;

public partial class BloodPressureListPage : ContentPage
{
    public BloodPressureListPage(BloodPressureViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}