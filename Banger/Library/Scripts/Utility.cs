using System;

namespace JuicyChicken
{
    public static class Utility
    {
        private static Random random = new Random();

        /// <summary>
        /// Returns true if chance is larger than a random value between 0 and 1
        /// </summary>
        public static bool Chance(float chance)
        {
            chance = Math.Clamp(chance, 0f, 1f);
            return chance > random.NextDouble();
        }

        /// <summary>
        /// Returns a random value from the specified array
        /// </summary>
        public static T Choose<T>(params T[] values)
        {
            return values[random.Next(0, values.Length)];
        }
    }
}
