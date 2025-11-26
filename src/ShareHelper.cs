using CommunityToolkit.Notifications;
using NearShare.Windows.Sender;
using System.Globalization;
using Windows.ApplicationModel.Internal.DataTransfer.NearShare;
using Windows.Storage;
using Windows.System.RemoteSystems;
using Windows.UI.Notifications;

namespace NearShare.Windows;

internal static class ShareHelper
{
    public static async Task SendFiles(RemoteSystem device, IEnumerable<IStorageItem> files)
    {
        await Transfer(
            device,
            title: string.Join(", ", files.Select(x => x.Name)),
            progress => NearShareSender.OutOfProcess.SendAsync(device, files, progress),
            showFileStatus: true
        ).ConfigureAwait(false);
    }

    public static async Task SendUri(RemoteSystem device, Uri uri)
    {
        await Transfer(
            device,
            title: uri.ToString(),
            progress => NearShareSender.OutOfProcess.SendAsync(device, uri, progress),
            showFileStatus: false
        ).ConfigureAwait(false);
    }

    private static async Task Transfer(RemoteSystem device, string title, Func<Progress<SendDataProgress>, Task<NearShareStatus>> transfer, bool showFileStatus)
    {
        Progress<SendDataProgress> progress = new();

        var content = new ToastContentBuilder()
            .AddText($"Transfer to \"{device.DisplayName}\"")
            .AddVisualChild(new AdaptiveProgressBar()
            {
                Title = title,
                Value = new BindableProgressBarValue("progressValue"),
                ValueStringOverride = new BindableString("progressValueString"),
                Status = new BindableString("progressStatus")
            })
            .GetToastContent();

        string tag = Guid.CreateVersion7().ToString();
        ToastNotification notification = new(content.GetXml())
        {
            Tag = tag,
            Data = new()
            {
                SequenceNumber = 0,
                Values =
                {
                    ["progressValue"] = "indeterminate",
                    ["progressStatus"] = Status(NearShareStatus.Unknown),
                    ["progressValueString"] = ""
                }
            }
        };
        ToastNotificationManager.CreateToastNotifier().Show(notification);

        progress.ProgressChanged += OnProgress;

        try
        {
            await transfer(progress).ConfigureAwait(false);
        }
        finally
        {
            progress.ProgressChanged -= OnProgress;
        }

        void OnProgress(object? sender, SendDataProgress e)
        {
            ToastNotificationManager.CreateToastNotifier().Update(data: new()
            {
                Values =
                {
                    ["progressValue"] = (showFileStatus, e.Status) switch
                    {
                        (showFileStatus: true, _) => (e.SentBytes / (double)e.TotalBytes).ToString(CultureInfo.InvariantCulture),
                        (showFileStatus: false, NearShareStatus.Completed) => "1.0",
                        _ => "indeterminate"
                    },
                    ["progressStatus"] = Status(e.Status),
                    ["progressValueString"] = showFileStatus ? $"{e.SentFiles}/{e.TotalFiles} Files" : ""
                }
            }, tag);
        }
    }

    private static string Status(NearShareStatus status) => status switch
    {
        NearShareStatus.Completed => "Completed",
        NearShareStatus.InProgress => "In Progress",
        NearShareStatus.TimedOut => "Timed Out",
        NearShareStatus.DeniedByRemoteSystem => "Denied by Remote System",
        NearShareStatus.VersionMismatch => "Version Mismatch",
        NearShareStatus.ConnectionEstablished => "Connection Established",
        NearShareStatus.NoSignedInUser => "No User signed-in",
        NearShareStatus.NetworkError => "Network Error",
        NearShareStatus.PlatformError => "Platform Error",
        _ => "Unknown",
    };
}
