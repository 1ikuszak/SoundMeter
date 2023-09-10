using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using SoundMeter.Services;
using SoundMeter.ViewModels;

namespace SoundMeter
{
    public class PopupControl : TemplatedControl
    {
        public PopupControl()
        {
            // Set the DataContext to the instance of this control (self)
            DataContext = new MainWindowViewModel(new DummyAudioInterfaceService());
        }
    }
}