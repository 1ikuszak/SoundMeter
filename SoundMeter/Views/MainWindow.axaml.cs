using System;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Threading;
using SoundMeter.Services;
using SoundMeter.ViewModels;

namespace SoundMeter.Views;

public partial class MainWindow : Window
{
    private Control mVolumeContainer;
    private Timer mSizingTimer;

    private void UpdateSizes()
    {
        var viewModel = (MainWindowViewModel)DataContext;
        viewModel.VolumeContainerSize = mVolumeContainer.Bounds.Height;
    }
    
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel(new DummyAudioInterfaceService());
        
        // find and handle VolumeContainer
        mVolumeContainer = this.FindControl<Control>("VolumeContainer") ?? throw new Exception("Cannot find Volume Container by name");
        mVolumeContainer.SizeChanged += (sender, e) =>
        {
            UpdateSizes();
        };
        
        // Start recording
        AudioCaptureService.DemoTwoChannel(4);
    }
}