using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System.Runtime.CompilerServices;
using Windows.UI;
using CompositionBrushOS = Windows.UI.Composition.CompositionBrush;
using CompositorOS = Windows.UI.Composition.Compositor;
using ICompositionSupportsSystemBackdropOS = Windows.UI.Composition.ICompositionSupportsSystemBackdrop;

namespace NearShare.Windows;

internal static partial class MicaHelper
{
    static readonly ConditionalWeakTable<Window, CustomMicaBackdrop> _backdrops = [];
    public static Window ApplyMica(this Window window)
    {
        if (window is not ICompositionSupportsSystemBackdropOS target)
            return window;

        if (!MicaController.IsSupported())
            return window;

        CustomMicaBackdrop backdrop = new();
        Wrapper wrapper = new(target);
        backdrop.AddTarget(
            wrapper,
            window.Content.XamlRoot
        );
        _backdrops.Add(window, backdrop);

        var ctx = SynchronizationContext.Current;

        _ = Task.Run(async () =>
        {
            await Task.Delay(2000);
            ctx?.Post((s) =>
            {
                var abc = backdrop.GetDefaultSystemBackdropConfiguration(wrapper, window.Content.XamlRoot);
                abc.IsInputActive = true;
                abc.IsHighContrast = false;
            }, null);
        });


        return window;
    }

    sealed partial class CustomMicaBackdrop : MicaBackdrop
    {
        public void AddTarget(ICompositionSupportsSystemBackdrop target, XamlRoot xamlRoot)
            => OnTargetConnected(target, xamlRoot);
    }

    sealed partial class Wrapper(ICompositionSupportsSystemBackdropOS target) : ICompositionSupportsSystemBackdrop
    {
        public CompositionBrushOS SystemBackdrop
        {
            get => target.SystemBackdrop;
            set
            {
                if (value is global::Windows.UI.Composition.CompositionColorBrush)
                    return;

                target.SystemBackdrop = null;
                // target.SystemBackdrop = value;
            }
        }
    }

    
}
