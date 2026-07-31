namespace TaskManagement.Mobile;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();
    }

    private async void OnBackToApplicationClicked(
        object? sender,
        EventArgs e)
    {
        await Navigation.PopAsync();
    }

}
