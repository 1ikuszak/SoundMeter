// using System;
// using System.Linq;
// using NAudio.Wave;
//
// namespace SoundMeter.Services;
//
// public class AudioCaptureService
// {
//     public static void DemoTwoChannel(int deviceID)
//     {
//         var waveIn = new WaveInEvent
//         {
//             DeviceNumber = deviceID,
//             WaveFormat = new WaveFormat(rate: 44100, bits: 16, channels: 2),
//             BufferMilliseconds = 20
//         };
//         waveIn.DataAvailable += WaveIn_DataAvailable;
//         waveIn.StartRecording();
//         // Console.WriteLine("(press any key to exit)");
//         // Console.ReadKey();
//     }
//
//     static void WaveIn_DataAvailable(object? sender, WaveInEventArgs e)
//     {
//         int bytesPerSample = 2;
//         int channelCount = 2;
//         int sampleCount = e.Buffer.Length / bytesPerSample / channelCount;
//
//         double[] values = new double[sampleCount];
//
//         for (int i = 0; i < sampleCount; i++)
//         {
//             int position = i * bytesPerSample * channelCount;
//             values[i] = BitConverter.ToInt16(e.Buffer, position) / 32768.0; // Normalize to range [-1.0, 1.0]
//         }
//
//         // Calculate RMS (Root Mean Square) for each channel
//         double rms = Math.Sqrt(values.Select(x => x * x).Average());
//
//         // Convert RMS to LUFS (assuming a reference of -23 LUFS)
//         double lufs = 20.0 * Math.Log10(rms) - 23.0;
//         
//         // Calculate level in decibels (dB)
//         double dB = 20.0 * Math.Log10(rms);
//
//         Console.CursorLeft = 0;
//         Console.CursorVisible = false;
//         Console.Write($"|  rms: {rms}  |  LUFS: {lufs:N2} LUFS  |  dB: {dB:N2} dB  |");
//     }
// }