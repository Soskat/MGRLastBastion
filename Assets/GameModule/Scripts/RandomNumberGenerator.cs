using UnityEngine;


namespace LastBastion.Game
{
    /// <summary>
    /// Class that provides random number generator methods.
    /// </summary>
    public class RandomNumberGenerator
    {
        #region Public methods
        /// <summary>
        /// Gets random float number from given range [min, max].
        /// </summary>
        /// <param name="min">Minimum value (inclusive)</param>
        /// <param name="max">Maximum value (inclusive)</param>
        /// <returns>Random float value</returns>
        public static float Range(float min, float max)
        {
            return Random.Range(min, max);
        }

        /// <summary>
        /// Gets random int number from given range [min, max).
        /// </summary>
        /// <param name="min">Minimum value (inclusive)</param>
        /// <param name="max">Maximum value (exclusive)</param>
        /// <returns>Random int value</returns>
        public static int Range(int min, int max)
        {
            return Random.Range(min, max);
        }
        #endregion
    }
}
