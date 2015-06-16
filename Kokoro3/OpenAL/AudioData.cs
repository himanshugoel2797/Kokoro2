#if OPENAL
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kokoro3.OpenAL
{
    public class AudioData : IDisposable
    {
        internal int ID;

        public AudioData()
        {
            ID = AL.GenBuffer();
            CheckError();
        }

        static void CheckError()
        {
            var error = AL.GetError();
            if (error != ALError.NoError) throw new Exception(AL.GetErrorString(error));
        }

        public void Load(Stream stream)
        {
            int channels, bits, rate;
            var data = LoadWave(stream, out channels, out bits, out rate);
            var format = GetSoundFormat(channels, bits);

            AL.BufferData(ID, format, data, data.Length, rate);
            CheckError();
        }

        #region Wave loading
        // Loads a wave/riff audio file.
        static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using (BinaryReader reader = new BinaryReader(stream))
            {
                // RIFF header
                string signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                int riff_chunck_size = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                string format_signature = new string(reader.ReadChars(4));
                if (format_signature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                int sample_rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();

                string data_signature = new string(reader.ReadChars(4));
                if (data_signature != "data")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int data_chunk_size = reader.ReadInt32();

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }
        }

        static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }
        #endregion

        public void Dispose()
        {
            AL.DeleteBuffer(ID);
            ID = 0;
            GC.SuppressFinalize(this);
        }

        ~AudioData()
        {
            System.Diagnostics.Debug.WriteLine($"[WARN] AudioData {ID} was automatically disposed");
            Dispose();
        }
    }
}
#endif