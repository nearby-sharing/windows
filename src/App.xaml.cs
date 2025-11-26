using Sentry.Protocol;
using System.Security;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NearShare.Windows;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        SentrySdk.Init(options =>
        {
            options.Dsn = "https://453b01729229e101db0dd1dfd9154428@o4506955567923200.ingest.us.sentry.io/4510108044230656";
#if DEBUG
            options.Debug = true;
#endif
            options.IsGlobalModeEnabled = true;
        });

        UnhandledException += OnUnhandledException;

        InitializeComponent();
        Suspending += OnSuspending;
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        Window window = Window.Current;

        if (window.Content is not Frame frame)
            window.Content = frame = new();

        frame.Navigate(typeof(MainPage), args.Arguments);

        window.Activate();
    }

    private async void OnSuspending(object sender, SuspendingEventArgs e)
    {
        var deferral = e.SuspendingOperation.GetDeferral();
        try
        {
            await SentrySdk.FlushAsync(timeout: TimeSpan.FromSeconds(2));
        }
        finally
        {
            deferral.Complete();
        }
    }

    [SecurityCritical]
    private void OnUnhandledException(object sender, global::Windows.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        var exception = e.Exception;
        if (exception is null)
            return;

        // Tell Sentry this was an unhandled exception
        exception.Data[Mechanism.HandledKey] = false;
        exception.Data[Mechanism.MechanismKey] = $"{nameof(Application)}.{nameof(UnhandledException)}";

        SentrySdk.CaptureException(exception);

        // Flush the event immediately
        SentrySdk.FlushAsync(timeout: TimeSpan.FromSeconds(2)).GetAwaiter().GetResult();
    }
}
