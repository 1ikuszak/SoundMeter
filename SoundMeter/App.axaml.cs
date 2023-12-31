using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SoundMeter.ViewModels;
using SoundMeter.Views;
using SoundMeter.Services; // Make sure to import your service namespace
using LiveChartsCore; 
using LiveChartsCore.Kernel; 
using LiveChartsCore.SkiaSharpView; 

namespace SoundMeter
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            LiveCharts.Configure(config => 
                config 
                    .HasMap<City>((city, point) => 
                    { 
                        point.Coordinate = new(point.Index, city.Population); 
                    }) 
            ); 
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
        
        public record City(string Name, double Population);
        
    }
}