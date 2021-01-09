using OpenTK;

namespace SharpGame.Objects.Components
{
    public class PositionComponent : Component
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public PositionComponent(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public void Set(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public void Translate(float x, float y, float z)
        {
            this.X += x;
            this.Y += y;
            this.Z += z;
        }

        public static implicit operator Vector3(PositionComponent positionComponent)
        {
            return new Vector3(positionComponent.X, positionComponent.Y, positionComponent.Z);
        } 
    }
}
