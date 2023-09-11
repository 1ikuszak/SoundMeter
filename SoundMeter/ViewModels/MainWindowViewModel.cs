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
        private int mUpdateCounter;
        
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
        private double _volumeContainerHeight;
        public double VolumeContainerHeigh
        {
            get => _volumeContainerHeight;
            set => this.RaiseAndSetIfChanged(ref _volumeContainerHeight, value);
        }
        private double _volumeBarHeight;
        public double VolumeBarHeight
        {
            get => _volumeBarHeight;
            set => this.RaiseAndSetIfChanged(ref _volumeBarHeight, value);
        }
        
        private double _volumeBarMaskHeight;
        public double VolumeBarMaskHeight
        {
            get => _volumeBarMaskHeight;
            set => this.RaiseAndSetIfChanged(ref _volumeBarMaskHeight, value);
        }
        
        public MainWindowViewModel(IAudioCaptureService audioCaptureService)
        {
            ExpanderButtonClickCommand = ReactiveCommand.Create<string>(text =>
            {
                // Update the selected channel
                SelectedChannel = text;
            });
            mAudioCaptureService = new NAdioCaptureService();
            Initialize(audioCaptureService);
        }
        private async void Initialize(IAudioCaptureService audioCaptureService)
        {
            var channelConfigurations = await mAudioCaptureService.GetChannelConfigurationAsync();
            ChannelConfigurations = channelConfigurations;
            StartCapture(0);
        }
        
        private void StartCapture(int deviceId)
        {
            mAudioCaptureService.InitCapture(deviceId);
            
            // Listen out for chunks of information
            mAudioCaptureService.AudioChunkAvailable += audioChunkData =>
            {
                ProcessAudioChunkForDisplay(audioChunkData);
            };
        
            // Start capturing
            mAudioCaptureService.Start();
        }

        private void ProcessAudioChunkForDisplay(AudioChunkData audioChunkData)
        {   
            // slow down display numbers
            mUpdateCounter = (mUpdateCounter + 1) % 3;
            if (mUpdateCounter == 0)
            {
                ShortTermLoudness = $"{Math.Max(-60, audioChunkData.ShortTermLUFS):0.0}";
                IntegratedLoudness  = $"{Math.Max(-60,audioChunkData.IntegratedLUFS):0.0}";
                LoudnessRange = $"{Math.Max(-60,audioChunkData.LoudnessRange):0.0}";
                RealtimeDynamics = $"{Math.Max(-60,audioChunkData.RealtimeDynamics):0.0}";
                AverageDynamics = $"{Math.Max(-60,audioChunkData.AverageRealtimeDynamics):0.0}";
                MomentaryMaxLoudness = $"{Math.Max(-60,audioChunkData.MomentaryMaxLUFS):0.0}";
                ShortTermMaxLoudness = $"{Math.Max(-60,audioChunkData.ShortTermMaxLUFS):0.0}";
                TruePeakMax = $"{Math.Max(-60,audioChunkData.TruePeakMax):0.0}";   
            }
            
            // Set volume bar height
            VolumeBarMaskHeight = Math.Min(_volumeBarHeight, _volumeBarHeight / 60 * -audioChunkData.ShortTermLUFS);
            // Set arrow position
            VolumePercentPosition = Math.Min(_volumeContainerHeight, _volumeContainerHeight / 60 * -audioChunkData.IntegratedLUFS);

        }
        

        public ICommand ExpanderButtonClickCommand { get; }

    }

}