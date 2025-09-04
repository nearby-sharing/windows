using Microsoft.UI.Xaml;
using Windows.ApplicationModel.Activation;

namespace NearShare.Windows;

public partial class App : IApplicationOverridesFeature_UwpSupportApi
{
    public void OnActivated(IActivatedEventArgs args)
    {
        if (args is not CommandLineActivatedEventArgs cmdArgs)
            return;

        cmdArgs.Operation.ExitCode = -1;
    }

    public void OnBackgroundActivated(BackgroundActivatedEventArgs args) { }

    public void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs args) { }

    public void OnFileActivated(FileActivatedEventArgs args) { }

    public void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args) { }

    public void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args) { }

    public void OnSearchActivated(SearchActivatedEventArgs args) { }

    public void OnShareTargetActivated(ShareTargetActivatedEventArgs args) { }

    public void OnWindowCreated(WindowCreatedEventArgs args) { }
}
