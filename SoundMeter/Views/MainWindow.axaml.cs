using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using NAudio.Wave;
using SoundMeter.Services;
using SoundMeter.ViewModels;

namespace SoundMeter.Views;

public partial class MainWindow : Window
{
    private Control mVolumeContainer;
    private Control mLabelControl;
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
        OnLoaded();
    }
    
    private void OnLoaded()
    {
        Task.Run(async () =>
        {
            // Output all devices, then select one
            foreach (var device in RecordingDevice.Enumerate())
                Console.WriteLine($"{device?.Index}: {device?.Name}");
            
            var outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NAudio");
            Directory.CreateDirectory(outputPath);
            var filePath = Path.Combine(outputPath, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".wav");
            using var writer = new WaveFileWriter(new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read), new WaveFormat());

            using var mCaptureDevice = new AudioCaptureTestService(0);

            mCaptureDevice.DataAvailable += (buffer, length) =>
            {
                writer.Write(buffer, 0, length);
                // Console.WriteLine(BitConverter.ToString(buffer));
            };

            mCaptureDevice.Start();
            await Task.Delay(8000);
            mCaptureDevice.Stop();
            await Task.Delay(100);
        });
    }

}
