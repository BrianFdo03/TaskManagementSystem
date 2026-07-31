namespace TaskManagement.Mobile;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(
        IActivationState? activationState)
    {
        var navigationPage = new NavigationPage(new MainPage());

        NavigationPage.SetHasNavigationBar(
            navigationPage,
            false);

        return new Window(navigationPage);
    }

}