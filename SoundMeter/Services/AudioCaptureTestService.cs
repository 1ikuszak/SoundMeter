using NAudio.Wave;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SoundMeter.Services;

public delegate void DataAvailableHandler(byte[] buffer, int length);

public class AudioCaptureTestService : IDisposable
{
    private WaveInEvent waveIn;
    private byte[] buffer;
    
    public event DataAvailableHandler DataAvailable;
    
    public AudioCaptureTestService(int deviceId)
    {
        // Initialize WaveInEvent with the specified device ID and audio format
        waveIn = new WaveInEvent
        {
            DeviceNumber = deviceId,
            WaveFormat = new WaveFormat(44100, 16, 2), // 44.1 kHz, 16-bit, Stereo
            BufferMilliseconds = 20
        };
        // Subscribe to the DataAvailable event
        waveIn.DataAvailable += WaveIn_DataAvailable;
    }
    
    private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
    {
        DataAvailable?.Invoke(e.Buffer, e.BytesRecorded);
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
