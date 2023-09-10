using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls.Primitives;
using ReactiveUI;
using SoundMeter.DataModels;
using SoundMeter.Services;

namespace SoundMeter.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private List<IGrouping<string, ChannelConfigurationItem>> _channelConfigurations;

        public List<IGrouping<string, ChannelConfigurationItem>> ChannelConfigurations
        {
            get => _channelConfigurations;
            set => this.RaiseAndSetIfChanged(ref _channelConfigurations, value);
        }
        
        
        private string _selectedChannel = "Select Channel"; // Initial value
        public string SelectedChannel
        {
            get => _selectedChannel;
            set => this.RaiseAndSetIfChanged(ref _selectedChannel, value);
        }
        
        public ICommand ExpanderButtonClickCommand { get; }
        
        
        public MainWindowViewModel(IAudioInterfaceService audioInterfaceService)
        {
            ExpanderButtonClickCommand = ReactiveCommand.Create<string>(text =>
            {
                // Update the selected channel
                SelectedChannel = text;
            });
            Initialize(audioInterfaceService);
        }

        private async void Initialize(IAudioInterfaceService audioInterfaceService)
        {
            var channelConfigurations = await audioInterfaceService.GetChannelConfigurationAsync();
            ChannelConfigurations = channelConfigurations;
        }
    }

}