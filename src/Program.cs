using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Win32;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using WinRT;

namespace NearShare.Windows;

public static class Program
{
    [MTAThread]
    static int Main()
    {
        // using var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\CDP");
        // var abc = key?.GetValue("NearShareWiFiDirectThresholdInBytes");

        return AppInstance.GetActivatedEventArgs() switch
        {
            CommandLineActivatedEventArgs cmdArgs => MainCli(cmdArgs),
            null => MainCli(null),
            _ => MainUI(),
        };
    }

    static int MainUI()
    {
        ComWrappersSupport.InitializeComWrappers();
        Application.Start((p) =>
        {
            DispatcherQueueSynchronizationContext context = new(DispatcherQueue.GetForCurrentThread());
            SynchronizationContext.SetSynchronizationContext(context);
            _ = new App();
        });
        return 0;
    }

    static int MainCli(CommandLineActivatedEventArgs? args)
    {
        Console.WriteLine("NearShare CLI is not implemented yet.");
        return -1;
    }
}
