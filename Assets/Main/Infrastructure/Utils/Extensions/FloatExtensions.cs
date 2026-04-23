using UnityEngine;

namespace Main.Infrastructure.Utils.Extensions
{
    public static class FloatExtensions
    {
        public static float Round(this float number, int digit = 3)
        {
            var divider = Mathf.Pow(10f, digit);
            return (int)(number * divider) / divider;
        }
    }
}
