using OpenTK;
using OpenTK.Input;
using OpenTK.Mathematics;

using SharpGame.Objects;
using SharpGame.Objects.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SharpGame.Objects.Components.Transform;
using Quaternion = OpenTK.Mathematics.Quaternion;
using Vector2 = OpenTK.Mathematics.Vector2;
using Vector3 = OpenTK.Mathematics.Vector3;

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

        public static Matrix4 CreateTransformationMatrix(TransformComponent transform)
        {
            Matrix4 scale = Matrix4.CreateScale(transform.Scale);

            Matrix4 rotation = Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(transform.Rotation.X,
                transform.Rotation.Y, transform.Rotation.Z));

            Matrix4 translation = Matrix4.CreateTranslation(transform.Position);

            return scale * rotation * translation;
        }
    }
}
