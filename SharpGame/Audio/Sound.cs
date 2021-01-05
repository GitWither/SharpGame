using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.ES10;

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

            byte[] audio = File.ReadAllBytes(path);
            AL.BufferData(soundId, ALFormat.Mono16, audio, audio.Length, 90000);
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
