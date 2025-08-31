using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Collections.ObjectModel;
using Windows.System.RemoteSystems;

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

    /// <summary>
    /// Gets or sets the DeviceList Selection Mode. Defaults to ListViewSelectionMode.Single
    /// </summary>
    public RemoteDeviceSelectionMode SelectionMode
    {
        get => (RemoteDeviceSelectionMode)GetValue(SelectionModeProperty);
        set => SetValue(SelectionModeProperty, value);
    }

    /// <summary>
    /// Gets the dependency property for <see cref="SelectionMode"/>.
    /// </summary>
    public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
        nameof(SelectionMode),
        typeof(RemoteDeviceSelectionMode),
        typeof(RemoteDevicePicker),
        new PropertyMetadata(RemoteDeviceSelectionMode.Single)
    );

    public RemoteSystemDiscoveryType DiscoveryType
    {
        get => (RemoteSystemDiscoveryType)GetValue(DiscoveryTypeProperty);
        set => SetValue(DiscoveryTypeProperty, value);
    }

    public static readonly DependencyProperty DiscoveryTypeProperty = DependencyProperty.Register(
        nameof(DiscoveryType),
        typeof(RemoteSystemDiscoveryType),
        typeof(RemoteDevicePicker),
        new PropertyMetadata(RemoteSystemDiscoveryType.SpatiallyProximal)
    );

    public RemoteSystemAuthorizationKind AuthorizationKind
    {
        get => (RemoteSystemAuthorizationKind)GetValue(AuthorizationKindProperty);
        set => SetValue(AuthorizationKindProperty, value);
    }

    public static readonly DependencyProperty AuthorizationKindProperty = DependencyProperty.Register(
        nameof(AuthorizationKind),
        typeof(RemoteSystemAuthorizationKind),
        typeof(RemoteDevicePicker),
        new PropertyMetadata(RemoteSystemAuthorizationKind.Anonymous)
    );

    public RemoteSystemStatusType StatusType
    {
        get => (RemoteSystemStatusType)GetValue(StatusTypeProperty);
        set => SetValue(StatusTypeProperty, value);
    }

    public static readonly DependencyProperty StatusTypeProperty = DependencyProperty.Register(
        nameof(StatusType),
        typeof(RemoteSystemStatusType),
        typeof(RemoteDevicePicker),
        new PropertyMetadata(RemoteSystemStatusType.Available)
    );

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoteDevicePicker"/> class.
    /// </summary>
    public RemoteDevicePicker()
    {
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
             new RemoteSystemDiscoveryTypeFilter(DiscoveryType),
             new RemoteSystemAuthorizationKindFilter(AuthorizationKind),
             new RemoteSystemStatusTypeFilter(StatusType)
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
