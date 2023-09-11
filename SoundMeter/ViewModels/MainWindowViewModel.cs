using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Microsoft.Win32;
using ReactiveUI;
using SoundMeter.DataModels;
using SoundMeter.Services;

namespace SoundMeter.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private IAudioCaptureService mAudioCaptureService;
        
        // Channel configuration list for correct menu
        private List<IGrouping<string, ChannelConfigurationItem>> _channelConfigurations;
        public List<IGrouping<string, ChannelConfigurationItem>> ChannelConfigurations
        {
            get => _channelConfigurations;
            set => this.RaiseAndSetIfChanged(ref _channelConfigurations, value);
        }

        #region Audio values

        private string _shortTermLoudness;
        public string ShortTermLoudness
        {
            get => _shortTermLoudness;
            set => this.RaiseAndSetIfChanged(ref _shortTermLoudness, value);
        }
        
        private string _integratedLoudness;
        public string IntegratedLoudness
        {
            get => _integratedLoudness;
            set => this.RaiseAndSetIfChanged(ref _integratedLoudness, value);
        }
        
        private string _loudnessRange;
        public string LoudnessRange
        {
            get => _loudnessRange;
            set => this.RaiseAndSetIfChanged(ref _loudnessRange, value);
        }
        
        private string _realtimeDynamics;
        public string RealtimeDynamics
        {
            get => _realtimeDynamics;
            set => this.RaiseAndSetIfChanged(ref _realtimeDynamics, value);
        }
        
        private string _averageDynamics;
        public string AverageDynamics
        {
            get => _averageDynamics;
            set => this.RaiseAndSetIfChanged(ref _averageDynamics, value);
        }
        
        private string _momentaryMaxLoudness;
        public string MomentaryMaxLoudness
        {
            get => _momentaryMaxLoudness;
            set => this.RaiseAndSetIfChanged(ref _momentaryMaxLoudness, value);
        }
        
        private string _shortTermMaxLoudness;
        public string ShortTermMaxLoudness
        {
            get => _shortTermMaxLoudness;
            set => this.RaiseAndSetIfChanged(ref _shortTermMaxLoudness, value);
        }
        
        private string _truePeakMax;
        public string TruePeakMax
        {
            get => _truePeakMax;
            set => this.RaiseAndSetIfChanged(ref _truePeakMax, value);
        }

        #endregion
        
        // Select button string value
        private string _selectedChannel = "Select"; // Initial value
        public string SelectedChannel
        {
            get => _selectedChannel;
            set => this.RaiseAndSetIfChanged(ref _selectedChannel, value);
        }
        
        // Volume arrow
        private double _volumePercentPosition;
        public double VolumePercentPosition
        {
            get => _volumePercentPosition;
            set => this.RaiseAndSetIfChanged(ref _volumePercentPosition, value);
        }
        
        // Volume Container
        private double _volumeContainerSize;
        public double VolumeContainerSize
        {
            get => _volumeContainerSize;
            set => this.RaiseAndSetIfChanged(ref _volumeContainerSize, value);
        }
        
        public MainWindowViewModel(IAudioCaptureService audioCaptureService)
        {
            ExpanderButtonClickCommand = ReactiveCommand.Create<string>(text =>
            {
                // Update the selected channel
                SelectedChannel = text;
            });
            UpdateArrowPosition();
            mAudioCaptureService = new NAdioCaptureService();
            Initialize(audioCaptureService);
        }
        
        private void UpdateArrowPosition()
        {
            var tick = 0;
            var input = 0.0;

            var tempTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1 / 120.0)
            };
            tempTimer.Tick += (sender, e) =>
            {
                tick++;
                input = tick / 20f;
                var scale = VolumeContainerSize / 2f;
                VolumePercentPosition = (Math.Sin(input) + 1) * scale;
            };
            tempTimer.Start();
        }

        private async void Initialize(IAudioCaptureService audioCaptureService)
        {
            var channelConfigurations = await mAudioCaptureService.GetChannelConfigurationAsync();
            ChannelConfigurations = channelConfigurations;
            StartCapture(0);
        }
        
        private void StartCapture(int deviceId)
        {
            mAudioCaptureService = new NAdioCaptureService(deviceId);
            
            // Listen out for chunks of information
            mAudioCaptureService.AudioChunkAvailable += audioChuckData =>
            {
                ShortTermLoudness = $"{audioChuckData.ShortTermLUFS:0.0} LUFS";
                IntegratedLoudness  = $"{audioChuckData.IntegratedLUFS:0.0} LUFS";
                LoudnessRange = $"{audioChuckData.LoudnessRange:0.0} LUFS";
                RealtimeDynamics = $"{audioChuckData.RealtimeDynamics:0.0} LUFS";
                AverageDynamics = $"{audioChuckData.AverageRealtimeDynamics:0.0} LUFS";
                MomentaryMaxLoudness = $"{audioChuckData.MomentaryMaxLUFS:0.0} LUFS";
                ShortTermMaxLoudness = $"{audioChuckData.ShortTermMaxLUFS:0.0} LUFS";
                TruePeakMax = $"{audioChuckData.TruePeakMax:0.0} dB";
            };
        
            // Start capturing
            mAudioCaptureService.Start();
        }

        public ICommand ExpanderButtonClickCommand { get; }

    }

}