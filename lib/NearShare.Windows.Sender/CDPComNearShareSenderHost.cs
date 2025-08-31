using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace NearShare.Windows.Sender;

internal static class CDPComNearShareSenderHost
{
    static readonly Guid CLSID = new("a4ed7ee3-e143-456d-8cc3-460a5303ad2b");
}

[GeneratedComInterface, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("fa2d2b3a-c8a1-4581-98d6-4f91a766f765")]
partial interface ICDPComNearShareSenderHost;
