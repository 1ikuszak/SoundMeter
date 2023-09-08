using Avalonia;
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

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            // Additional code for handling template changes if needed
        }
    }
}