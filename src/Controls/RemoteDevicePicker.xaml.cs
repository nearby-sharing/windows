using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Windows.System.RemoteSystems;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace NearShare.Windows.Controls;

/// <summary>
/// Picker Control to show List of Remote Devices that are accessible
/// </summary>
public sealed partial class RemoteDevicePicker : ContentDialog
{
    /// <summary>
    /// Gets or sets List of All Remote Systems based on Selection Filter
    /// </summary>
    private ObservableCollection<RemoteSystem> RemoteSystems { get; set; } = [];

    public RemoteDevicePickerOptions Options { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteDevicePicker"/> class.
    /// </summary>
    public RemoteDevicePicker(RemoteDevicePickerOptions? options = null)
    {
        Options = options ?? new();

        InitializeComponent();
        BuildFilters();
    }

    /// <summary>
    /// Initiate Picker UI
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task<IEnumerable<RemoteSystem>> PickDeviceAsync()
    {
        return await ShowAsync() switch
        {
            ContentDialogResult.Primary => _listDevices.SelectedItems.Cast<RemoteSystem>(),
            _ => []
        };
    }

    private void Filters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        => BuildFilters();

    private void BuildFilters()
    {
        RemoteDeviceHelper remoteDeviceHelper = new([
             new RemoteSystemDiscoveryTypeFilter(Options.DiscoveryType),
             new RemoteSystemAuthorizationKindFilter(Options.AuthorizationKind),
             new RemoteSystemStatusTypeFilter(Options.StatusType)
        ]);
        RemoteSystems = remoteDeviceHelper.RemoteSystems;

        UpdateProgressRing(true);
        _listDevices.ItemsSource = RemoteSystems;
        UpdateProgressRing(false);
    }

    private void ListDevices_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
    {
        var model = (RemoteSystem)args.Item;
        switch (model.Status)
        {
            case RemoteSystemStatus.Available:
                args.ItemContainer.IsEnabled = true;
                args.ItemContainer.IsHitTestVisible = true;
                break;

            case RemoteSystemStatus.DiscoveringAvailability:
            case RemoteSystemStatus.Unavailable:
            case RemoteSystemStatus.Unknown:
                args.ItemContainer.IsEnabled = false;
                args.ItemContainer.IsHitTestVisible = false;
                break;
        }
    }

    private void ListDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        IsPrimaryButtonEnabled = _listDevices.SelectedItems.Count > 0;
    }

    private void ListDevices_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        // ToDo
    }

    private void UpdateProgressRing(bool state)
    {
        // ToDo
        //if (_progressRing != null)
        //{
        //    _progressRing.IsActive = state;
        //}
    }
}

public sealed partial class RemoteDevicePickerOptions : ObservableObject
{
    /// <summary>
    /// Gets or sets the DeviceList Selection Mode. Defaults to ListViewSelectionMode.Single
    /// </summary>
    [ObservableProperty]
    public partial RemoteDeviceSelectionMode SelectionMode { get; set; } = RemoteDeviceSelectionMode.Single;

    [ObservableProperty]
    public partial RemoteSystemDiscoveryType DiscoveryType { get; set; } = RemoteSystemDiscoveryType.Any;

    [ObservableProperty]
    public partial RemoteSystemAuthorizationKind AuthorizationKind { get; set; } = RemoteSystemAuthorizationKind.Anonymous;

    [ObservableProperty]
    public partial RemoteSystemStatusType StatusType { get; set; } = RemoteSystemStatusType.Available;
}
