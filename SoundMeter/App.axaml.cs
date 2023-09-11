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
            // Initialize the dependencies
            var audioInterface = new NAdioCaptureService();
            var mainWindowViewModel = new MainWindowViewModel(audioInterface);
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainWindowViewModel
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainWindow()
                {
                    DataContext = mainWindowViewModel
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}