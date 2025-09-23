using Windows.System.RemoteSystems;

namespace NearShare.Windows.Controls;

public sealed class RemoteSystemAuthorizationKindInfo : IEnumDisplayInfo
{
    public RemoteSystemAuthorizationKind AuthorizationKind { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string IconData { get; set; } = null!;
}
