// using NAudio.Wave;
// using System;
// using System.Runtime.InteropServices;
// using System.Threading.Tasks;
//
// namespace SoundMeter.Services;
//
// public delegate void DataAvailableHandler(byte[] buffer, int length);
//
// public class AudioCaptureTestService : IDisposable
// {
//     private WaveInEvent waveIn;
//     private byte[] buffer;
//     
//     public event DataAvailableHandler DataAvailable;
//     
//     public AudioCaptureTestService(int deviceId, int frequency = 44100)
//     {
//         // Initialize WaveInEvent with the specified device ID and audio format
//         waveIn = new WaveInEvent
//         {
//             DeviceNumber = deviceId,
//             WaveFormat = new WaveFormat(frequency, 16, 2), // 44.1 kHz, 16-bit, Stereo
//             BufferMilliseconds = 20
//         };
//         // Subscribe to the DataAvailable event
//         waveIn.DataAvailable += WaveIn_DataAvailable;
//     }
//     
//     private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
//     {
//         DataAvailable?.Invoke(e.Buffer, e.BytesRecorded);
//     }
//     
//     public void Start()
//     {
//         waveIn.StartRecording();
//     }
//  
//     public void Stop()
//     {
//         waveIn.StopRecording();
//     }
//     
//     public void Dispose()
//     {
//         // Stop recording and release resources
//         waveIn.StopRecording();
//         waveIn.Dispose();
//     }
// }
//
// // var outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NAudio");
// // Directory.CreateDirectory(outputPath);
// // var filePath = Path.Combine(outputPath, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".wav");
// // using var writer = new WaveFileWriter(new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read), new WaveFormat());