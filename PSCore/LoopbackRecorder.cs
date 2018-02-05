using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
using CSCore.MediaFoundation;
using CSCore.CoreAudioAPI;
using CSCore.Streams;

namespace PSCore
{
    public class LoopbackRecorder
    {
        public static WasapiCapture capture;
        public static SoundInSource wasapiCaptureSource;
        public static IWaveSource stereoSource;
        public static MediaFoundationEncoder w;

        public static void StartRecording(String fileName, int bitRate = 192000)
        {
            capture = new WasapiLoopbackCapture();

            capture.Initialize();

            wasapiCaptureSource = new SoundInSource(capture);
            stereoSource = wasapiCaptureSource.ToStereo();


            if (fileName.EndsWith(".mp3"))
            {
                w = MediaFoundationEncoder.CreateMP3Encoder(stereoSource.WaveFormat, fileName, bitRate);
            }
            else if (fileName.EndsWith(".wma"))
            {
                w = MediaFoundationEncoder.CreateWMAEncoder(stereoSource.WaveFormat, fileName, bitRate);
            }
            else if (fileName.EndsWith(".aac"))
            {
                w = MediaFoundationEncoder.CreateAACEncoder(stereoSource.WaveFormat, fileName, bitRate);
            }

            capture.DataAvailable += (s, e) =>
            {
                w.Write(e.Data, e.Offset, e.ByteCount);
            };

            capture.Start();
        }

        public static void StopRecording()
        {
            capture.Stop();
        }

        public static void Dispose()
        {
            w.Dispose();
            stereoSource.Dispose();
            wasapiCaptureSource.Dispose();
            capture.Dispose();
        }
    }
}
