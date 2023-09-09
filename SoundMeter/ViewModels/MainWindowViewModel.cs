using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using SoundMeter.DataModels;
using SoundMeter.Services;

namespace SoundMeter.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<IGrouping<string, ChannelConfigurationItem>> _channelConfigurations;

        public List<IGrouping<string, ChannelConfigurationItem>> ChannelConfigurations
        {
            get => _channelConfigurations;
            set => this.RaiseAndSetIfChanged(ref _channelConfigurations, value);
        }
        

        public MainWindowViewModel(IAudioInterfaceService audioInterfaceService)
        {
            Initialize(audioInterfaceService);
        }

        private async void Initialize(IAudioInterfaceService audioInterfaceService)
        {
            var channelConfigurations = await audioInterfaceService.GetChannelConfigurationAsync();
            ChannelConfigurations = channelConfigurations;
        }
    }

}