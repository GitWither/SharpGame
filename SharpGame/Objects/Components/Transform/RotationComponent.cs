using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Objects.Components
{
    public class RotationComponent : Component
    {
        public float Pitch { get; private set; }
        public float Yaw { get; private set; }
        public float Roll { get; private set; }

        public RotationComponent(float pitch, float yaw, float roll)
        {
            this.Pitch = pitch;
            this.Yaw = yaw;
            this.Roll = roll;
        }

        public void Set(float pitch, float yaw, float roll)
        {
            this.Pitch = pitch;
            this.Yaw = yaw;
            this.Roll = roll;
        }

        public void Rotate(float pitch, float yaw, float roll)
        {
            this.Pitch += pitch;
            this.Yaw += yaw;
            this.Roll += roll;
        }
    }
}
