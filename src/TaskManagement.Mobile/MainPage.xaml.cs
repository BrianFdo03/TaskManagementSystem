namespace TaskManagement.Mobile;

public partial class MainPage : ContentPage
{
    private const string WebAppUrl = "http://192.168.1.243:5173/?platform=maui";

    public MainPage()
    {
        InitializeComponent();

        WebView.Navigating += OnWebViewNavigating;
        WebView.Navigated += OnWebViewNavigated;

        LoadWebApp();
    }

    private void LoadWebApp()
    {
        ErrorView.IsVisible = false;
        LoadingView.IsVisible = true;

        WebView.Source = WebAppUrl;
    }

    private async void OnWebViewNavigating(
        object? sender,
        WebNavigatingEventArgs e)
    {
        /*
         * Handle navigation requests from the Vue application.
         */
        if (e.Url.StartsWith("maui://"))
        {
            e.Cancel = true;

            await HandleNativeNavigation(e.Url);

            return;
        }

        LoadingView.IsVisible = true;
        ErrorView.IsVisible = false;
    }

    private async Task HandleNativeNavigation(string url)
    {
        if (url.Equals(
            "maui://about",
            StringComparison.OrdinalIgnoreCase))
        {
            await Navigation.PushAsync(new AboutPage());
        }
    }

    private void OnWebViewNavigated(
        object? sender,
        WebNavigatedEventArgs e)
    {
        LoadingView.IsVisible = false;

        if (e.Result != WebNavigationResult.Success)
        {
            ShowError();
        }
    }

    private void ShowError()
    {
        LoadingView.IsVisible = false;
        ErrorView.IsVisible = true;

        ErrorMessage.Text =
            "The web application could not be loaded. " +
            "Please check your network connection.";
    }

    private void OnRetryClicked(
        object? sender,
        EventArgs e)
    {
        LoadWebApp();
    }

    protected override bool OnBackButtonPressed()
    {
        if (WebView.CanGoBack)
        {
            WebView.GoBack();

            return true;
        }

        return base.OnBackButtonPressed();
    }

    //private async void OnAboutClicked(
    //object? sender,
    //EventArgs e)
    //{
    //    await Navigation.PushAsync(new AboutPage());
    //}
}