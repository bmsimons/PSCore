using System;
using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
using CSCore.MediaFoundation;
using CSCore.Streams;

namespace PSCore
{
    public enum WriterType
    {
        EncoderWriter,
        WaveWriter
    };

    public class LoopbackRecorder
    {
        public static WasapiCapture capture;
        public static SoundInSource wasapiCaptureSource;
        public static IWaveSource stereoSource;
        public static MediaFoundationEncoder encoderWriter;
        public static WaveWriter waveWriter;
        public static WriterType writerType;

        public static void StartRecording(String fileName, int bitRate = 192000)
        {
            capture = new WasapiLoopbackCapture();

            capture.Initialize();

            wasapiCaptureSource = new SoundInSource(capture);
            stereoSource = wasapiCaptureSource.ToStereo();

            switch (System.IO.Path.GetExtension(fileName))
            {
                case ".mp3":
                    encoderWriter = MediaFoundationEncoder.CreateMP3Encoder(stereoSource.WaveFormat, fileName, bitRate);
                    writerType = WriterType.EncoderWriter;
                    break;
                case ".wma":
                    encoderWriter = MediaFoundationEncoder.CreateWMAEncoder(stereoSource.WaveFormat, fileName, bitRate);
                    writerType = WriterType.EncoderWriter;
                    break;
                case ".aac":
                    encoderWriter = MediaFoundationEncoder.CreateAACEncoder(stereoSource.WaveFormat, fileName, bitRate);
                    writerType = WriterType.EncoderWriter;
                    break;
                case ".wav":
                    waveWriter = new WaveWriter(fileName, capture.WaveFormat);
                    writerType = WriterType.WaveWriter;
                    break;
            }

            switch (writerType)
            {
                case WriterType.EncoderWriter:
                    capture.DataAvailable += (s, e) =>
                    {
                        encoderWriter.Write(e.Data, e.Offset, e.ByteCount);
                    };
                    break;
                case WriterType.WaveWriter:
                    capture.DataAvailable += (s, e) =>
                    {
                        waveWriter.Write(e.Data, e.Offset, e.ByteCount);
                    };
                    break;
            }

            // Start recording
            capture.Start();
        }

        public static void StopRecording()
        {
            // Stop recording
            capture.Stop();

            // Dispose respective writers (for WAV, Dispose() writes header)
            switch (writerType)
            {
                case WriterType.EncoderWriter:
                    encoderWriter.Dispose();
                    break;
                case WriterType.WaveWriter:
                    waveWriter.Dispose();
                    break;
            }

            // Dispose of other objects
            stereoSource.Dispose();
            wasapiCaptureSource.Dispose();
            capture.Dispose();
        }
    }
}
