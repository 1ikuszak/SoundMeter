using Avalonia;
using Avalonia.Controls.Primitives;

namespace SoundMeter;

public class LargeLabelControl : TemplatedControl
{
    public static readonly StyledProperty<string> LargeTextProperty = AvaloniaProperty.Register<LargeLabelControl, string>(
        nameof(LargeText), defaultValue:"SMALL");

    public string LargeText
    {
        get => GetValue(LargeTextProperty);
        set => SetValue(LargeTextProperty, value);
    }

    public static readonly StyledProperty<string> UnitProperty = AvaloniaProperty.Register<LargeLabelControl, string>(
        nameof(Unit), defaultValue:"LUTS");

    public string Unit
    {
        get => GetValue(UnitProperty);
        set => SetValue(UnitProperty, value);
    }

    public static readonly StyledProperty<string> SmallTextProperty = AvaloniaProperty.Register<LargeLabelControl, string>(
        nameof(SmallText), defaultValue:"LARGE");

    public string SmallText
    {
        get => GetValue(SmallTextProperty);
        set => SetValue(SmallTextProperty, value);
    }
}