using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NearShare.Windows.Controls;
using NearShare.Windows.Sender;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Internal.DataTransfer.NearShare;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System;
using Windows.System.RemoteSystems;

namespace NearShare.Windows;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
    }

    #region Send

    private async void SendFileButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            FileOpenPicker picker = new()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                ViewMode = PickerViewMode.Thumbnail,
                FileTypeFilter = { "*" }
            };
            if (await picker.PickMultipleFilesAsync() is not { Count: > 0 } files)
                return;

            await SendFilesAsync(files);
        }
        catch (Exception ex)
        {
            ShowErrorDialog(ex);
        }
    }

    private async void SendClipBoardButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var clipboard = Clipboard.GetContent();
            if (clipboard.Contains(StandardDataFormats.StorageItems))
            {
                var items = await clipboard.GetStorageItemsAsync();
                await SendFilesAsync(items);
            }
            else if (clipboard.Contains(StandardDataFormats.Text))
            {
                var text = await clipboard.GetTextAsync();
                await SendTextAsync(text);
            }
            else if (clipboard.Contains(StandardDataFormats.Uri))
            {
                var uri = await clipboard.GetUriAsync();
                await SendUriAsync(uri);
            }
        }
        catch (Exception ex)
        {
            ShowErrorDialog(ex);
        }
    }

    private async void SendTextButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ContentDialog dialog = new()
            {
                Title = "Send Text",
                PrimaryButtonText = "Send",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = new TextBox
                {
                    AcceptsReturn = true,
                    TextWrapping = TextWrapping.Wrap,
                    Height = 200,
                    Width = 400
                }.Ref(out var textBox),
                XamlRoot = Content.XamlRoot
            };
            if (await dialog.ShowAsync() != ContentDialogResult.Primary)
                return;

            await SendTextAsync(textBox.Text);
        }
        catch (Exception ex)
        {
            ShowErrorDialog(ex);
        }
    }

    async Task SendTextAsync(string text)
    {
        if (Uri.TryCreate(text.Trim(), UriKind.Absolute, out var uri))
        {
            await SendUriAsync(uri);
            return;
        }

        StorageFile file = await StorageFile.CreateStreamedFileAsync(
            $"Text-Transfer-{DateTime.Now:dd_MM_yyyy-HH_mm_ss}.txt",
            async (stream) =>
            {
                using DataWriter writer = new(stream);
                writer.WriteString(text);
                await writer.StoreAsync();
                await writer.FlushAsync();
            },
            thumbnail: null
        );
        await SendFilesAsync(file);
    }

    async Task SendFilesAsync(params IEnumerable<IStorageItem> files)
    {
        var device = await PickRemoteSystemAsync();
        if (device is null)
            return;

        Progress<SendDataProgress> progress = new();
        await NearShareSender.OutOfProcess.SendAsync(device, files, progress);
    }

    async Task SendUriAsync(Uri uri)
    {
        var device = await PickRemoteSystemAsync();
        if (device is null)
            return;

        Progress<SendDataProgress> progress = new();
        await NearShareSender.OutOfProcess.SendAsync(device, uri, progress);
    }

    async Task<RemoteSystem?> PickRemoteSystemAsync()
    {
        RemoteDevicePicker picker = new()
        {
            XamlRoot = Content.XamlRoot
        };
        return (await picker.PickDeviceAsync()).FirstOrDefault();
    }

    #endregion Send

    private async void OpenSettingsButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:crossdevice"));
        }
        catch { }
    }

    private async void AboutSettingsCard_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await Launcher.LaunchUriAsync(new Uri("https://nearshare.shortdev.de"));
        }
        catch { }
    }

    private async void GitHubSettingsCard_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/nearby-sharing/"));
        }
        catch { }
    }

    async void ShowErrorDialog(Exception ex)
    {
        ContentDialog dialog = new()
        {
            Title = "Error",
            PrimaryButtonText = "OK",
            DefaultButton = ContentDialogButton.Primary,
            Content = new TextBlock
            {
                Text = ex.Message,
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 400
            },
            XamlRoot = Content.XamlRoot
        };
        try
        {
            await dialog.ShowAsync();
        }
        catch { }
    }
}
