using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoundMeter.DataModels;

namespace SoundMeter.Services;

public interface IAudioCaptureService
{
    /// <summary>
    /// Fetch the channel configuration
    /// </summary>
    /// <returns></returns>
    Task<List<IGrouping<string, ChannelConfigurationItem>>> GetChannelConfigurationAsync();

    void InitCapture(int deviceId = 1, int frequency = 44100);
    
    void Start();
    void Stop();

    event Action<AudioChunkData> AudioChunkAvailable;
}   