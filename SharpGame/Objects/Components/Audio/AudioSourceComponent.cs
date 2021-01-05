using SharpGame.Audio;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public class AudioSourceComponent : Component
    {
        public Sound Sound { get; set; }
        public float Volume
        {
            set => this.Sound.SetVolume(value);
        }
        public float Pitch
        {
            set => this.Sound.SetPitch(value);
        }
        public float Distance
        {
            set => this.Sound.SetMaxDistance(value);
        }
        public bool Looping
        {
            set => this.Sound.SetLooping(value);
        }
        public AudioSourceComponent(Sound sound, float volume, float pitch, float distance, bool looping)
        {
            this.Sound = sound;
            this.Distance = distance;
            this.Pitch = pitch;
            this.Volume = volume;
            this.Looping = looping;
            this.Sound.Play();
        }

        public override void OnUpdate(float deltaTime)
        {
            this.Sound.SetPosition(this.Actor.PositionComponent.X, this.Actor.PositionComponent.Y, this.Actor.PositionComponent.Z);
            this.Sound.SetListenerData(SharpGameWindow.ActiveScene.Camera.Actor.PositionComponent.X, SharpGameWindow.ActiveScene.Camera.Actor.PositionComponent.Y, SharpGameWindow.ActiveScene.Camera.Actor.PositionComponent.Z);
        }
    }
}
