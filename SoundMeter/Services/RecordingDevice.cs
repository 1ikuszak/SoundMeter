using NAudio.Wave;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoundMeter.Services;

public class RecordingDevice : IDisposable
{
    public int Index { get; }
    public string Name { get; }

    private WaveInEvent waveIn;

    private RecordingDevice(int index, string name)
    {
        Index = index;
        Name = name;
    }

    public static IEnumerable<RecordingDevice> Enumerate()
    {
        for (int n = 0; n < WaveInEvent.DeviceCount; n++)
        {
            var caps = WaveInEvent.GetCapabilities(n);
            yield return new RecordingDevice(n, caps.ProductName);
        }
    }

    public void Dispose()
    {
        StopRecording();
    }

    public void StartRecording()
    {
        if (waveIn == null)
        {
            waveIn = new WaveInEvent();
            waveIn.DeviceNumber = Index;
        }

        waveIn.StartRecording();
    }

    public void StopRecording()
    {
        waveIn.StopRecording();
        waveIn.Dispose();
        waveIn = null;
    }

    public override string ToString() => Name;
}


// foreach (var device in RecordingDevice.Enumerate())
//     Console.WriteLine($"{device?.Index}: {device?.Name}");