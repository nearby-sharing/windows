using Windows.System.RemoteSystems;

namespace NearShare.Windows.Controls;

public sealed class RemoteSystemDiscoveryTypeInfo : IEnumDisplayInfo
{
    public RemoteSystemDiscoveryType DiscoveryType { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string IconData { get; set; } = null!;
}
