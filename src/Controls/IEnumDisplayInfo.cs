using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace NearShare.Windows.Controls;

public interface IEnumDisplayInfo
{
    string DisplayName { get; set; }
    string IconData { get; set; }

    Geometry Icon => XamlBindingHelper.ConvertValue(typeof(Geometry), IconData) as Geometry ?? Geometry.Empty;
}
