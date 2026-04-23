using UnityEngine;

namespace Main.Infrastructure.Utils.Extensions
{
    public static class NumberExtensions
    {
        private const float _DEFAULT_PRECISION = 0.001f;

        public static float Lerp(float a, float b, float t, bool extrapolate = false)
        {
            if (!extrapolate)
            {
                t = Mathf.Clamp01(t);
            }

            return t * b + (1 - t) * a;
        }
        
        public static bool Approximately(this float a, float b, float precision = _DEFAULT_PRECISION)
        {
            return a < b + precision && a > b - precision;
        }
        
        public static bool Approximately(this Vector3 v1, Vector3 v2)
        {
            return v1.x.Approximately(v2.x) && v1.y.Approximately(v2.y) && v1.z.Approximately(v2.z);
        }
        
        public static int Compare(this int a, int b)
        {
            if (a < b)
            {
                return 1;
            }
            
            if (a > b)
            {
                return -1;
            }

            return 0; // Equals
        }

        public static long ToPow(this int baseNumber, int toPower)
        {
            long result = 1;
            while (toPower > 0)
            {
                result *= baseNumber;
                toPower--;
            }
            
            return result;
        }
    }
}