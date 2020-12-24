using OpenTK;
using OpenTK.Input;

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
    }
}
