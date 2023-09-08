using System.Collections.Generic;
using System.Threading.Tasks;
using SoundMeter.DataModels;

namespace SoundMeter.Services;

public interface IAudioInterfaceService
{
    /// <summary>
    /// Fetch the channel configuration
    /// </summary>
    /// <returns></returns>
    Task<List<ChannelConfigurationItem>> GetChannelConfigurationAsync();
}