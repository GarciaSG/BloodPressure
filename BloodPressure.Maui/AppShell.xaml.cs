using BloodPressure.Maui.Views;

namespace BloodPressure.Maui;

public partial class AppShell : Shell
{
    public AppShell(LoginView loginview)
    {
        InitializeComponent();

        MainShellContent.Content = loginview;

        Routing.RegisterRoute(nameof(BloodPressureFormPage), typeof(BloodPressureFormPage));
        Routing.RegisterRoute(nameof(BloodPressureListPage), typeof(BloodPressureListPage));
        Routing.RegisterRoute("register", typeof(CreateUserView));
    }
}