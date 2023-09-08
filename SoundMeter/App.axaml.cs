using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SoundMeter.ViewModels;
using SoundMeter.Views;
using SoundMeter.Services; // Make sure to import your service namespace

namespace SoundMeter
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var audioInterfaceService = new DummyAudioInterfaceService(); // Create an instance of your service
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(audioInterfaceService),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}