using OpenTK;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.ES10;
using OpenTK.Graphics.OpenGL;

using SharpGame.Util;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Audio
{
    public class Sound : IDisposable
    {
        private readonly int soundId;
        private readonly int sourceId;

        public Sound(string path)
        {
            soundId = AL.GenBuffer();
            sourceId = AL.GenSource();

            AL.Source(sourceId, ALSourcef.RolloffFactor, 1);
            AL.Source(sourceId, ALSourcef.ReferenceDistance, 1);

            using (NVorbis.VorbisReader vorbis = new NVorbis.VorbisReader(path))
            {
                /*
                if (vorbis.Channels > 1)
                {
                    Logger.Error($"Failed to load sound {path}. Must have 1 channel.");
                    return;
                }
                */

                float[] readBuffer = new float[vorbis.Channels * vorbis.SampleRate / 5];

                vorbis.ClipSamples = false;

                int readSamples = vorbis.ReadSamples(readBuffer, 0, readBuffer.Length);

                short[] parsedBuffer = new short[readBuffer.Length];

                //byte[] data = readBuffer.SelectMany(value => BitConverter.GetBytes(value)).ToArray();
                for (int i = 0; i < readBuffer.Length; i++)
                {
                    parsedBuffer[i] = (short)MathHelper.Clamp((int)(short.MaxValue * readBuffer[i]), short.MinValue, short.MaxValue);
                }

                AL.BufferData(soundId, vorbis.Channels == 1 ? ALFormat.Mono16 : ALFormat.Stereo16, parsedBuffer, readSamples * sizeof(short), vorbis.SampleRate);
            }
        }

        public void SetPitch(float pitch)
        {
            AL.Source(sourceId, ALSourcef.Pitch, pitch);
        }

        public void SetVolume(float volume)
        {
            AL.Source(sourceId, ALSourcef.Gain, volume);
        }

        public void SetListenerData(float x, float y, float z)
        {
            AL.Listener(ALListener3f.Position, x, y, z);
        }

        public void SetPosition(float x, float y, float z)
        {
            AL.Source(sourceId, ALSource3f.Position, x, y, z);
        }

        public void SetVelocity(float x, float y, float z)
        {
            AL.Source(sourceId, ALSource3f.Velocity, x, y, z);
        }

        public void SetLooping(bool looping)
        {
            AL.Source(sourceId, ALSourceb.Looping, looping);
        }

        public void SetMaxDistance(float distance)
        {
            AL.Source(sourceId, ALSourcef.MaxDistance, distance);
        }

        public void Play()
        {
            AL.Source(sourceId, ALSourcei.Buffer, soundId);
            AL.SourcePlay(sourceId);
        }

        public void Pause()
        {
            AL.SourcePause(sourceId);
        }

        public void Resume()
        {
            AL.SourcePlay(sourceId);
        }

        public void Stop()
        {
            AL.SourceStop(sourceId);
        }

        public bool IsPlaying()
        {
            return AL.GetSourceState(sourceId) == ALSourceState.Playing;
        }

        public void Dispose()
        {
            Stop();
            AL.DeleteSource(sourceId);
            AL.DeleteBuffer(soundId);
        }
    }
}
