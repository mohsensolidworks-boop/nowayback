using UnityEngine;

namespace Main.Infrastructure.Utils.Extensions
{
    public static class OtherExtensions
    {
        public static Quaternion ShortestRotation(this Quaternion a, Quaternion b)
        {
            if (Quaternion.Dot(a, b) < 0)
            {
                return a * Quaternion.Inverse(b.QuatMultiply(-1));
            }
            else
            {
                return a * Quaternion.Inverse(b);
            }
        }
        
        public static Quaternion QuatMultiply(this Quaternion input, float scalar)
        {
            return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
        }
        
        public static Color Alpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}
