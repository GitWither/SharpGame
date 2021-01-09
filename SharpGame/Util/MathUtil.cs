using OpenTK;
using OpenTK.Input;

using SharpGame.Objects.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Util
{
    internal static class MathUtil
    {
        public static bool Vector3ApproximatelyEqual(Vector3 a, Vector3 b)
        {
            return MathHelper.ApproximatelyEqualEpsilon(a.X, b.X, SharedConstants.VectorMargin) &&
                   MathHelper.ApproximatelyEqualEpsilon(a.Y, b.Y, SharedConstants.VectorMargin) &&
                   MathHelper.ApproximatelyEqualEpsilon(a.Z, b.Z, SharedConstants.VectorMargin);
        }

        public static bool Vector2ApproximatelyEqual(Vector2 a, Vector2 b)
        {
            return MathHelper.ApproximatelyEqualEpsilon(a.X, b.X, SharedConstants.VectorMargin) &&
                   MathHelper.ApproximatelyEqualEpsilon(a.Y, b.Y, SharedConstants.VectorMargin);
        }

        public static Matrix4 CreateTransformationMatrix(PositionComponent positionComponent, RotationComponent rotationComponent, ScaleComponent scaleComponent)
        {
            Matrix4 scale = Matrix4.CreateScale(scaleComponent.X, scaleComponent.Y, scaleComponent.Z);

            Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotationComponent.Roll)) *
                               Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotationComponent.Yaw)) *
                               Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotationComponent.Pitch));

            Matrix4 translation = Matrix4.CreateTranslation(positionComponent);

            return translation * rotation * scale;
        }
    }
}
