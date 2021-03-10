using OpenTK;

namespace SharpGame.Particles
{
    struct Particle
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public float Lifetime { get; set; }

        public void Reset()
        {
            this.Lifetime = 0;
            this.Position = Vector3.Zero;
        }
    }
}
