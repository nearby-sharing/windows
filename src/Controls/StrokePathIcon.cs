using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using PathShape = Microsoft.UI.Xaml.Shapes.Path;

namespace NearShare.Windows.Controls;

// https://github.com/microsoft/microsoft-ui-xaml/blob/adae19431771d29b4c94d7a2bfd889e7d8c0fa4c/src/dxaml/xcp/core/core/elements/icon.cpp#L925-L958
internal sealed partial class StrokePathIcon : PathIcon
{
    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var path = this.FindDescendant<PathShape>() ?? throw new UnreachableException("Base class should have added a path as child");
        path.Fill = null;
        path.Stroke = Foreground;
    }
}
