using OpenTK;
using OpenTK.Input;
using OpenTK.Mathematics;

using SharpGame.Objects;
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

        public static Matrix4 CreateTransformationMatrix(Actor actor)
        {
            Matrix4 scale = Matrix4.CreateScale(actor.ScaleComponent.X, actor.ScaleComponent.Y, actor.ScaleComponent.Z);

            Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(actor.RotationComponent.Roll)) *
                               Matrix4.CreateRotationY(MathHelper.DegreesToRadians(actor.RotationComponent.Yaw)) *
                               Matrix4.CreateRotationX(MathHelper.DegreesToRadians(actor.RotationComponent.Pitch));

            Matrix4 translation = Matrix4.CreateTranslation(actor.PositionComponent.X, actor.PositionComponent.Y, actor.PositionComponent.Z);

            return translation * rotation * scale;
        }
    }
}
