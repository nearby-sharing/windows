using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using NearShare.Windows;
using WinRT;

ComWrappersSupport.InitializeComWrappers();
Application.Start((p) =>
{
    DispatcherQueueSynchronizationContext context = new(DispatcherQueue.GetForCurrentThread());
    SynchronizationContext.SetSynchronizationContext(context);
    _ = new App();
});
