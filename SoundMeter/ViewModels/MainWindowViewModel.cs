using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ReactiveUI;
using SoundMeter.DataModels;
using SoundMeter.Services;

namespace SoundMeter.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IAudioInterfaceService _audioInterfaceService;

        public MainWindowViewModel(IAudioInterfaceService audioInterfaceService)
        {
            _audioInterfaceService = audioInterfaceService;
            LoadChannelConfiguration();
        }

        private ObservableCollection<ChannelConfigurationItem> _channelConfigurations;
        public ObservableCollection<ChannelConfigurationItem> ChannelConfigurations
        {
            get => _channelConfigurations;
            set => this.RaiseAndSetIfChanged(ref _channelConfigurations, value);
        }

        public async Task LoadChannelConfiguration()
        {
            var configurations = await _audioInterfaceService.GetChannelConfigurationAsync();
            ChannelConfigurations = new ObservableCollection<ChannelConfigurationItem>(configurations);
        }

        public string Greeting => "Welcome to Avalonia!";
    }
}