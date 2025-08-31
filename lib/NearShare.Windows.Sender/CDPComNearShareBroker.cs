using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Windows.ApplicationModel.Internal.DataTransfer.NearShare;
using Windows.Win32.System.Com;
using static Windows.Win32.PInvoke;

namespace NearShare.Windows.Sender;

public static class CDPComNearShareBroker
{
    static readonly Guid CLSID = new("96274226-3195-4cde-b0a0-0f6256c7a65a");

    public static unsafe ShareSenderBroker CreateNearShareSender()
    {
        // ToDo: Is ref-counting valid here?

        CoCreateInstance(in CLSID, pUnkOuter: null, (CLSCTX)0x100004u, typeof(ICDPComNearShareBroker).GUID, out var pBroker).ThrowOnFailure();
        try
        {
            var broker = ComInterfaceMarshaller<ICDPComNearShareBroker>.ConvertToManaged(pBroker) ?? throw new InvalidOperationException("Broker was null");
            var sender = broker.CreateNearShareSender();
            return ShareSenderBroker.FromAbi((nint)ComInterfaceMarshaller<ICDPComNearShareSender>.ConvertToUnmanaged(sender));
        }
        finally
        {
            ComInterfaceMarshaller<ICDPComNearShareBroker>.Free(pBroker);
        }
    }
}

[GeneratedComInterface, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("6b8007ae-4dd7-46f3-9bec-06f777d78864")]
partial interface ICDPComNearShareBroker
{
    ICDPComNearShareHandler CreateNearShareHandler(in Guid a, [MarshalAs(UnmanagedType.LPStr)] string b);
    ICDPComNearShareSender CreateNearShareSender();
}

[GeneratedComInterface, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("1a975b05-870a-4e8f-b910-f0986d481bab")]
partial interface ICDPComNearShareSender;

[GeneratedComInterface, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("a7224b82-30f6-497a-be79-58e991488b67")]
partial interface ICDPComNearShareHandler;
