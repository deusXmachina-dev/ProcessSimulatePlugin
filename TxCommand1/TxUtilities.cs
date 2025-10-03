using System;
using Tecnomatix.Engineering;

namespace TxCommand1
{
    /// <summary>
    /// Utility class for working with Tecnomatix objects.
    /// </summary>
    public static class TxUtilities
    {
        /// <summary>
        /// Calculates the magnitude (length) of a TxVector.
        /// </summary>
        /// <param name="vector">The vector to calculate magnitude for.</param>
        /// <returns>The magnitude of the vector.</returns>
        public static double Magnitude(this TxVector vector)
        {
            if (vector == null)
                throw new ArgumentNullException(nameof(vector));

            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
        }

        /// <summary>
        /// Calculates the 3D Euclidean distance between two TxVectors.
        /// </summary>
        /// <param name="from">The starting vector.</param>
        /// <param name="to">The ending vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public static double Distance(TxVector from, TxVector to)
        {
            if (from == null)
                throw new ArgumentNullException(nameof(from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));

            double dx = to.X - from.X;
            double dy = to.Y - from.Y;
            double dz = to.Z - from.Z;

            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Calculates the vertical (Z-axis) distance between two TxVectors.
        /// </summary>
        /// <param name="from">The starting vector.</param>
        /// <param name="to">The ending vector.</param>
        /// <returns>The absolute vertical distance.</returns>
        public static double VerticalDistance(TxVector from, TxVector to)
        {
            if (from == null)
                throw new ArgumentNullException(nameof(from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));

            return Math.Abs(to.Z - from.Z);
        }
    }
}
