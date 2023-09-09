using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoundMeter.DataModels;

namespace SoundMeter.Services;

public interface IAudioInterfaceService
{
    /// <summary>
    /// Fetch the channel configuration
    /// </summary>
    /// <returns></returns>
    Task<List<IGrouping<string, ChannelConfigurationItem>>> GetChannelConfigurationAsync();
}