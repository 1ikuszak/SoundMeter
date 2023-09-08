using Avalonia.Controls;
using SoundMeter.Services;
using SoundMeter.ViewModels;

namespace SoundMeter.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel(new DummyAudioInterfaceService());
    }
}