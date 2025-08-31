using Microsoft.CorrelationVector;
using System.Diagnostics.CodeAnalysis;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Internal.DataTransfer.NearShare;
using Windows.Storage;
using Windows.System.RemoteSystems;

namespace NearShare.Windows.Sender;

public sealed class NearShareSender(ShareSenderBroker broker)
{
    readonly ShareSenderBroker _broker = broker;

    public async Task<NearShareStatus> SendAsync(RemoteSystem receiver, Uri uri, IProgress<SendDataProgress> progress, CancellationToken cancellationToken = default)
    {
        DataPackage package = new();
        package.SetUri(uri);

        CorrelationVector correlationVector = new CorrelationVectorV1();
        return await _broker.ShareDataWithProgressAsync(correlationVector.Value, receiver, package, StandardDataFormats.Uri).AsTask(cancellationToken, progress);
    }

    public async Task<NearShareStatus> SendAsync(RemoteSystem receiver, IEnumerable<IStorageItem> storageItems, IProgress<SendDataProgress> progress, CancellationToken cancellationToken = default)
    {
        DataPackage package = new();
        package.SetStorageItems(storageItems);

        CorrelationVector correlationVector = new CorrelationVectorV1();
        return await _broker.ShareDataWithProgressAsync(correlationVector.Value, receiver, package, StandardDataFormats.StorageItems).AsTask(cancellationToken, progress);
    }

    [field: MaybeNull]
    public static NearShareSender InProcess => field ??= new(new ShareSenderBroker());

    [field: MaybeNull]
    public static NearShareSender OutOfProcess => field ??= new(CDPComNearShareBroker.CreateNearShareSender());
}
