using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicData.List;
using SoundMeter.DataModels;

namespace SoundMeter.Services;

public class DummyAudioInterfaceService : IAudioInterfaceService
{
    public Task<List<System.Linq.IGrouping<string, ChannelConfigurationItem>>> GetChannelConfigurationAsync()
    {
        var items = new List<ChannelConfigurationItem>(new[]
        {
            new ChannelConfigurationItem("Mono Stereo Configuration", "Mono", "Mono"),
            new ChannelConfigurationItem("Mono Stereo Configuration", "Stereo", "Stereo"),
            new ChannelConfigurationItem("5.1 Surround", "5.1 DTS - (L, R, Ls, Rs, C, LFE)", "5.1 DTS"),
            new ChannelConfigurationItem("5.1 Surround", "5.1 DTS - (L, R, C, LFR, Ls, Rs)", "5.1 ITU"),
            new ChannelConfigurationItem("5.1 Surround", "5.1 DTS - (L, C, R, Ls, Rs, LFE)", "5.1 FILM")
        });

        var groupedItems = items.GroupBy(item => item.Group).ToList();
        return Task.FromResult(groupedItems);
    }

}