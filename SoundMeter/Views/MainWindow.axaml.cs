using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using NAudio.Wave;
using NWaves.Signals;
using NWaves.Utils;
using SoundMeter.Services;
using SoundMeter.ViewModels;

namespace SoundMeter.Views;

public partial class MainWindow : Window
{
    public MainWindowViewModel mWindowViewModel => (MainWindowViewModel)DataContext;
    private Control mVolumeContainer;
    private Control mLabelControl;
    private Timer mSizingTimer;

    private void UpdateSizes()
    {
        mWindowViewModel.VolumeContainerSize = mVolumeContainer.Bounds.Height;
    }


    public MainWindow()
    {
        InitializeComponent();
        // find and handle VolumeContainer
        mVolumeContainer = this.FindControl<Control>("VolumeContainer") ??
                           throw new Exception("Cannot find Volume Container by name");
        mVolumeContainer.SizeChanged += (sender, e) => { UpdateSizes(); };
    }
    
}
