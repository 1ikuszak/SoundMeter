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
        // Channel configuration list for correct menu
        private List<IGrouping<string, ChannelConfigurationItem>> _channelConfigurations;
        public List<IGrouping<string, ChannelConfigurationItem>> ChannelConfigurations
        {
            get => _channelConfigurations;
            set => this.RaiseAndSetIfChanged(ref _channelConfigurations, value);
        }
        
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
        
        public MainWindowViewModel(IAudioInterfaceService audioInterfaceService)
        {
            ExpanderButtonClickCommand = ReactiveCommand.Create<string>(text =>
            {
                // Update the selected channel
                SelectedChannel = text;
            });
            UpdateArrowPosition();
            Initialize(audioInterfaceService);
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

        private async void Initialize(IAudioInterfaceService audioInterfaceService)
        {
            var channelConfigurations = await audioInterfaceService.GetChannelConfigurationAsync();
            ChannelConfigurations = channelConfigurations;
            
        }
        
        public ICommand ExpanderButtonClickCommand { get; }

    }

}