using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NWaves.Signals;
using NWaves.Utils;
using SoundMeter.DataModels;


namespace SoundMeter.Services;

public class NAdioCaptureService : IDisposable, IAudioCaptureService
{
    private WaveInEvent waveIn;
    private byte[] mBuffer;
    private int mDevice;
    private Queue<double> mLufs = new Queue<double>();
    private int mCaptureFrequency = 44100;
    public event Action<AudioChunkData> AudioChunkAvailable;

    public NAdioCaptureService(int deviceId = 0, int frequency = 44100)
    {
        // Initialize WaveInEvent with the specified device ID and audio format
        waveIn = new WaveInEvent
        {
            DeviceNumber = deviceId,
            WaveFormat = new WaveFormat(frequency, 16, 2), // 44.1 kHz, 16-bit, Stereo
            BufferMilliseconds = 20
        };
        // Subscribe to the DataAvailable event to handle captured audio data
        waveIn.DataAvailable += AudioChunkCaptured;
    }
    
    public Task<List<System.Linq.IGrouping<string, ChannelConfigurationItem>>> GetChannelConfigurationAsync()
    {
        var items = new List<ChannelConfigurationItem>(new[]
        {
            new ChannelConfigurationItem("Mono Stereo Configuration", "Mono", "MONO"),
            new ChannelConfigurationItem("Mono Stereo Configuration", "Stereo", "STEREO"),
            new ChannelConfigurationItem("5.1 Surround", "5.1 DTS - (L, R, Ls, Rs, C, LFE)", "5.1 DTS"),
            new ChannelConfigurationItem("5.1 Surround", "5.1 DTS - (L, R, C, LFR, Ls, Rs)", "5.1 ITU"),
            new ChannelConfigurationItem("5.1 Surround", "5.1 DTS - (L, C, R, Ls, Rs, LFE)", "5.1 FILM")
        });

        var groupedItems = items.GroupBy(item => item.Group).ToList();
        return Task.FromResult(groupedItems);
    }
    
    
    private void AudioChunkCaptured(object sender, WaveInEventArgs e)
    {
        // You can access captured audio data in e.Buffer
        CalculateValues(e.Buffer);
    }
    
    
    private void CalculateValues(byte[] buffer)
    {
        // Get total PCM16 samples in this buffer (16 bits per sample)
        var sampleCount = buffer.Length / 2;

        // Create our Discrete Signal ready to be filled with information
        var signal = new DiscreteSignal(mCaptureFrequency, sampleCount);

        // Loop all bytes and extract the 16 bits, into signal floats
        using var reader = new BinaryReader(new MemoryStream(buffer));

        for (var i = 0; i < sampleCount; i++)
            signal[i] = reader.ReadInt16() / 32768f;
            
        // Calculate the LUFS
        var lufs = Scale.ToDecibel(signal.Rms() * 1.2);
        mLufs.Enqueue(lufs);
            
        // Keep list to 10 samples
        if (mLufs.Count > 100)
            mLufs.Dequeue();

        // Calculate the average
        var averageLufs = mLufs.Average();

        // Fire off this chunk of information to listeners
        AudioChunkAvailable?.Invoke(new AudioChunkData
        (
            Loudness: lufs,

            ShortTermLUFS: averageLufs,
            IntegratedLUFS:  lufs * 0.9,
            LoudnessRange: lufs * 0.8,
            RealtimeDynamics:  lufs * 0.7,
            AverageRealtimeDynamics:  lufs * 0.6,
            MomentaryMaxLUFS:  lufs * 0.5,
            ShortTermMaxLUFS:  lufs * 0.4,
            TruePeakMax:  lufs * 0.3
            ));
    }
    
    public void Start()
    {
        waveIn.StartRecording();
    }
 
    public void Stop()
    {
        waveIn.StopRecording();
    }
    
    public void Dispose()
    {
        // Stop recording and release resources
        waveIn.StopRecording();
        waveIn.Dispose();
    }
}